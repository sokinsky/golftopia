using Azure;
using Azure.Core;
using Castle.Core.Logging;
using GTA.Client.Repositories;
using GTA.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GTA.Client {
    public interface IRepository<out TModel, out TController> : IEnumerable<TController> {    
        Type ModelType { get; }
        Type ControllerType { get; }
        Context Context { get; }
        string Name { get; }
    }
    public class Repository<TModel, TController> : IRepository<TModel, TController>
        where TModel:Model, new()
        where TController:Controller<TModel> {
        public Repository(Context context) { 
            this.Context = context;
            this.Name = this.Context.GetType().GetProperties().Single(x => x.PropertyType == this.GetType()).Name;
        }
        public Type ModelType => typeof(TModel);
        public Type ControllerType => typeof(TController);
        public Context Context { get; }
        public string Name { get; }
        protected List<TController> controllers { get; } = new List<TController>();
        public IEnumerator<TController> GetEnumerator() { return ((IEnumerable<TController>)controllers).GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable)controllers).GetEnumerator(); }

        #region Http
        protected async Task<HttpResponseMessage> get(int id) {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Name}/{id}"),
                Method = HttpMethod.Get,
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;
        }
        public async Task<HttpResponseMessage> post(string methodName) {
            return await this.post(methodName, new { });
        }
        public async Task<HttpResponseMessage> post(string methodName, object parameters) {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.Context.BaseAddress}/{this.Name.ToLower()}/{methodName.ToLower()}"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")
            };
            var responseMessage = await this.Context.SendAsync(requestMessage);
            return responseMessage;
        }
        #endregion

        public TController Add(TController item) {
            var controller = this.SingleOrDefault(x => x == item);
            if (controller == null) {
                controller = this.SingleOrDefault(x => x.UID == item.UID);
                if (controller == null) {
                    controller = this.SingleOrDefault(x => x.Model == item.Model);
                    if (controller == null) {
                        controller = this.SingleOrDefault(x => x.Model.ID == item.Model.ID);
                        if (controller == null) {
                            controller = item;
                            this.controllers.Add(controller);
                        }
                    }
                }
            }
            return controller;

        }
        public TController Add(TModel model) {
            var controllerInstance = Activator.CreateInstance(typeof(TController), new object[] { this.Context, model });
            if (controllerInstance == null)
                throw new Exception("Unable to create controller");
            return this.Add((TController)controllerInstance);
        }

        public TController? Find(TModel model) {
            var result = this.SingleOrDefault(x => x.Model == model);
            if (result == null) 
                result = this.SingleOrDefault(x => x.ID == model.ID);
            return result;
        }
        public TController? Find(Guid guid) {
            return this.SingleOrDefault(x => x.UID == guid);
        }
        public TController? Find(int id) {
            return this.SingleOrDefault(x => x.ID == id);
        }

        public async Task<TController?> FindAsync(int id) {
            var result = this.Find(id);       
            if (result != null)
                return result;

            return await this.AddAsync(await this.get(id));
        }
        public async Task<TController?> AddAsync(HttpResponseMessage responseMessage) {
            var content = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(content);
            var model = JsonSerializer.Deserialize<TModel>(content);
            if (model == null)
                throw new Exception($"Unable to deserialize {typeof(TModel).Name}.  Invalid json");
            var result = this.Find(model);
            if (result == null)
                result = this.Add(model);
            else
                result.Reload(model);
            return result;
        }
    }
}
