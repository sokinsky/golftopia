using GTA.Client.Controllers;
using GTA.Client.Repositories;
using GTA.Data;
using GTA.Data.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GTA.Client {
    public class Context : HttpClient, IEnumerable<IRepository<Model, IController<Model>>> {
        public Context() {
            this.BaseAddress = new Uri("https://localhost:44313/api");
            this.onRepositoryBuilder();            
        }
        public Context(Configuration configuration) {
            this.BaseAddress = new Uri(configuration.api.url);
            this.onRepositoryBuilder();
        }

        #region Repositories
        private List<IRepository<Model, IController<Model>>> repositories { get; set; } = new List<IRepository<Model, IController<Model>>>();
        public IEnumerator<IRepository<Model, IController<Model>>> GetEnumerator() { return ((IEnumerable<IRepository<Model, IController<Model>>>)repositories).GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable)repositories).GetEnumerator(); }

        public IRepository<Model, IController<Model>> Find(string name) {
            return this.repositories.Single(x => x.Name == name);
        }
        public IRepository<Model, IController<Model>> Find<TModel>() where TModel:Model, new() {
            return this.repositories.Single(x => x.ModelType == typeof(TModel));
        }

        public LoginController? Login { get; set; }
        public UserController? User => this.Login?.User;

        public PersonRepository People => (PersonRepository)this.Find<Person>();
        public UserRepository Users => (UserRepository)this.Find<User>();
        public LoginRepository Logins => (LoginRepository)this.Find<Login>();
        #endregion

        #region Http
        public async Task<HttpResponseMessage> Post(string methodName) {
            return await this.Post(methodName, new { });
        }
        public async Task<HttpResponseMessage> Post(string methodName, object parameters) {
            var requestMessage = new HttpRequestMessage {
                RequestUri = new Uri($"{this.BaseAddress}/{methodName.ToLower()}"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")
            };
            var responseMessage = await this.SendAsync(requestMessage);
            return responseMessage;
        }
        #endregion

        protected virtual void onRepositoryBuilder() {
            foreach (var propertyInfo in this.GetType().GetProperties().Where(x => typeof(IRepository<Model, IController<Model>>).IsAssignableFrom(x.PropertyType))) {
                var repositoryInstance = Activator.CreateInstance(propertyInfo.PropertyType, new object[] { this });
                if (repositoryInstance != null) {
                    var repository = (IRepository<Model, IController<Model>>)repositoryInstance;
                    this.repositories.Add(repository);
                }
            }
        }

        public class Configuration {
            public API api { get; set; } = new API();
            public class API {
                public string url { get; set; } = "https://localhost:44313/api";

            }
        }
    }
}
