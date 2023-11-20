using GTA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Text.Json;

namespace GTA.Server {
    public static class Repository
    {
        public static IRepository<Model, IController<Model>> Create<TModel>(Context context) where TModel : Model, new()
        {
            return Create(context, typeof(TModel));
        }
        public static IRepository<Model, IController<Model>> Create(Context context, Type modelType)
        {
            var propertyInfos = context.GetType().GetProperties().Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) && x.PropertyType.GenericTypeArguments.Single() == modelType);
            switch (propertyInfos.Count())
            {
                case 0:
                    throw Web.Exception.NotFound($"Unable to locate repository for Model({modelType.Name})");
                case 1:
                    return Create(context, propertyInfos.Single());
                default:
                    throw Web.Exception.BadRequest($"Ambiguous repositories for Model({modelType.Name})");
            }
        }
        public static IRepository<Model, IController<Model>> Create(Context context, string propertyName)
        {
            var propertyInfos = context.GetType().GetProperties().Where(x => x.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            switch (propertyInfos.Count())
            {
                case 0:
                    throw Web.Exception.NotFound($"{propertyName} is not a valid repository name");
                case 1:
                    return Create(context, propertyInfos.Single());
                default:
                    propertyInfos = context.GetType().GetProperties().Where(x => x.Name == propertyName);
                    switch (propertyInfos.Count())
                    {
                        case 0:
                            throw Web.Exception.NotFound($"{propertyName} is not a valid repository name");
                        case 1:
                            return Create(context, propertyInfos.Single());
                        default:
                            throw Web.Exception.BadRequest($"Repository name({propertyName}) was not unique within the context");
                    }
            }
        }
        public static IRepository<Model, IController<Model>> Create(Context context, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                throw new Exception($"Invalid Property.  Property({propertyInfo.Name}) must be a DbSet<>");
            if (!propertyInfo.PropertyType.GenericTypeArguments.Single().IsAssignableFrom(typeof(Model)))
                throw new Exception($"Invalid Property.  Property({propertyInfo.Name}) must be a DbSet<Model>");
            var modelType = propertyInfo.PropertyType.GenericTypeArguments.Single();
            var repositoryAttribute = modelType.GetCustomAttribute<RepositoryAttribute>();
            if (repositoryAttribute == null)
                throw new Exception($"Invalid Model. Model is missing a Repository Attreibute");
            var result = Activator.CreateInstance(repositoryAttribute.Type, new object[] { context });
            if (result == null)
                throw new Exception("Unable to create repository");
            var repository = (IRepository<Model, IController<Model>>)result;
            repository.Name = propertyInfo.Name;
            return repository;


        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RepositoryAttribute : Attribute
    {
        public RepositoryAttribute(Type type) { Type = type; }
        public Type Type { get; set; }
    }
    public interface IRepository<out TModel, out TController>
        where TModel : Model, new()
        where TController : IController<TModel>
    {

        Context Context { get; }
        string Name { get; set; }
        Type ModelType { get; }
        Type ControllerType { get; }

        TController Add(object model);
        TController Remove(int id);        
    }
    public class Repository<TModel, TController> : IRepository<TModel, TController>
        where TModel : Model, new()
        where TController : IController<TModel> {

        public Repository(Context context) { Context = context; }
        public Context Context { get; }
        public string Name { get; set; } = default!;
        public Type ModelType => typeof(TModel);
        public Type ControllerType => typeof(TController);

        public IEnumerable<IKey> Keys {
            get {
                var entityType = this.Context.Model.FindEntityType(typeof(TModel));
                if (entityType == null)
                    throw new Exception($"Type({typeof(TModel).Name}) is not an entity");
                var results = entityType.GetKeys();
                return results;
            }
        }

        public TController Add(object model) {
            var addModel = JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(model));
            if (addModel == null)
                throw new Exception("Unable to create model");
            return this.Add(addModel);
        }
        public TController Add(TModel model) {
            var entity = this.Context.Set<TModel>().Add(model);
            return this[entity.Entity];
        }

        public TController Remove(TModel model) {
            var entity = this.Context.Set<TModel>().Remove(model);
            return this[entity.Entity];
        }
        public TController Remove(int id) {
            var model = this.Context.Set<TModel>().Find(id);
            if (model == null)
                throw Web.Exception.NotFound($"Unable to remove {typeof(TModel)}. It does not exist");
            return this.Remove(model);
        }

        [Web.Method]
        public async Task<TController> FindAsync(object model) {
            switch (model) {
                case TModel tModel:
                    return await this.FindAsync(tModel);
                default:
                    var data = JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(model));
                    if (data == null)
                        throw new Exception("");
                    return await this.FindAsync(data);
            }
        }
        [Web.Method]
        public async Task<TController> FindAsync(int id) {
            return await this.FindAsync(new TModel { ID = id });
        }
        public async Task<TController> FindAsync(TModel data) {
            var model = await this.Context.Set<TModel>().FindAsync(data.ID);
            if (model == null)
                throw Web.Exception.NotFound($"There is not {typeof(TModel).Name} with ID:{data.ID}");
            return this[model];
        }

        [Web.Method]
        public async Task<IEnumerable<TController>> Search() {
            var models = await this.Context.Set<TModel>().ToListAsync();
            return models.Select(model => this[model]);
        }
        [Web.Method]
        public async Task<IEnumerable<TController>> Search(Paging paging) {
            var skip = (paging.Number - 1) * paging.Size;
            var take = paging.Size;
            var models = await this.Context.Set<TModel>().Skip(skip).Take(take).ToListAsync();
            return models.Select(model => this[model]);
        }


        public TController this[TModel model] {
            get {
                var result = Activator.CreateInstance(typeof(TController), new object[] { this.Context, model });
                if (result == null)
                    throw new Exception("Unable to create Controller");
                return (TController)result;
            }
        }
    }

    public class Paging {
        public int Size { get; set; } = 100;
        public int Number { get; set; } = 1;

    }
}
