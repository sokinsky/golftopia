using GTA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GTA.Client {

    public interface IController<out TModel> where TModel:Model, new() {
        Guid UID { get; }
        Context Context { get; }
        TModel Model { get; }
        public int ID { get; }
        Type ModelType { get; }
        IRepository<Model, IController<Model>> Repository { get; }
    }
    public class Controller<TModel> : Dictionary<PropertyInfo, object?>,  IController<TModel> where TModel:Model, new() {
        public Guid UID { get; } = Guid.NewGuid();
        public Controller(Context context, TModel model) {
            this.Context = context;
            this.Model = model;
            var propertyInfos = typeof(TModel).GetProperties().Where(x => x.SetMethod != null && x.SetMethod.IsPublic);
            propertyInfos = propertyInfos.Where(x => x.GetMethod != null && x.GetMethod.IsPublic);
            propertyInfos = propertyInfos.Where(x => x.GetCustomAttribute<NotMappedAttribute>() == null);
            propertyInfos = propertyInfos.Where(x => !typeof(IEnumerable<Model>).IsAssignableFrom(x.PropertyType));
            propertyInfos = propertyInfos.Where(x => !typeof(Model).IsAssignableFrom(x.PropertyType));
            foreach (var propertyInfo in propertyInfos) {
                this[propertyInfo] = propertyInfo.GetValue(this.Model);
            }
        }
        public Type ModelType => typeof(TModel);
        public Context Context { get; }
        public TModel Model { get; }
        public int ID => this.Model.ID;
        public EntityState ModelState { 
            get {
                switch (this.ID) {
                    case 0: return EntityState.Added;
                    default:
                        foreach (var propertyInfo in this.Keys) {
                            var propertyValue = propertyInfo.GetValue(this.Model);
                            if (propertyValue == null) {
                                if (this[propertyInfo] != null)
                                    return EntityState.Modified;
                            }
                            else if (!propertyValue.Equals(this[propertyInfo]))
                                return EntityState.Modified;
                        }
                        return EntityState.Unchanged;
                }
            }
        }

        public IRepository<Model, IController<Model>> Repository => this.Context.Find<TModel>();

        #region Http

        protected async Task<HttpResponseMessage> get() {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Repository.Name.ToLower()}/{this.ID}"),
                Method = HttpMethod.Get
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;
        }
        protected async Task<HttpResponseMessage> patch() {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Repository.Name.ToLower()}/{this.ID}"),
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonSerializer.Serialize(this.Model), Encoding.UTF8, "application/json")
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;
        }
        protected async Task<HttpResponseMessage> put() {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Repository.Name.ToLower()}"),
                Method = HttpMethod.Put,
                Content = new StringContent(JsonSerializer.Serialize(this.Model), Encoding.UTF8, "application/json")
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;

        }
        protected async Task<HttpResponseMessage> delete() {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Repository.Name.ToLower()}/{this.Model.ID}"),
                Method = HttpMethod.Delete
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;

        }
        protected async Task<HttpResponseMessage> post(string methodName) {
            return await this.post(methodName, new { });
        }
        protected async Task<HttpResponseMessage> post(string methodName, object parameters) {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Repository.Name.ToLower()}/{methodName.ToLower()}"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;
        }

        public void Reload(TModel model) {
            foreach (var propertyInfo in this.Keys) {
                propertyInfo.SetValue(this.Model, propertyInfo.GetValue(model));
                this[propertyInfo] = propertyInfo.GetValue(this.Model);
            }
        }

        public async Task Load(HttpResponseMessage responseMessage) {
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Unable to load an unsuccessful response");
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(responseContent);
            this.Load(jsonElement);
        }
        public virtual void Load(JsonElement jsonObject) {
            if (jsonObject.ValueKind != JsonValueKind.Object)
                throw new Exception("Invalid Token:  Object expected");
            foreach (var jsonProperty in jsonObject.EnumerateObject()) {
                var propertyInfo = typeof(TModel).GetProperty(jsonProperty.Name);
                if (propertyInfo == null)
                    throw new Exception($"Unable to load data. {typeof(TModel).Name}.{jsonProperty.Name} could not be found");
                if (propertyInfo.SetMethod == null || !propertyInfo.SetMethod.IsPublic)
                    throw new Exception($"Unable to load {typeof(TModel).Name}.{jsonProperty.Name}. No setter available");
                this.load(propertyInfo, jsonProperty);
            }
        }
        protected virtual void load(PropertyInfo propertyInfo, JsonProperty jsonProperty) {
            if (typeof(Model).IsAssignableFrom(propertyInfo.PropertyType))
                return;
            if (typeof(IEnumerable<Model>).IsAssignableFrom(propertyInfo.PropertyType))
                return;
            var value = JsonSerializer.Deserialize(jsonProperty.Value, propertyInfo.PropertyType);
            this[propertyInfo] = value;
            propertyInfo.SetValue(this.Model, value);
        }

        #endregion

        protected async Task insert() {
            var responseMessage = await this.put();
            if (responseMessage.IsSuccessStatusCode) {
                await this.Load(responseMessage);
            }
        }
        protected async Task update() {
            var responseMessage = await this.patch();
            if (responseMessage.IsSuccessStatusCode) {
                await this.Load(responseMessage);
            }
        }
        public async Task Save() {
            if (this.ID == 0)
                await this.insert();
            else
                await this.update();
            
        }

        public async Task Delete() {
            var responseMessage = await this.delete();
            if (responseMessage.IsSuccessStatusCode) {
                await this.Load(responseMessage);
            }
        }
    }
}
