using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LMS.Data.Server {
    public static class Repository {
        public static IRepository<Context, Model, IController<Context, Model>> Create<TModel>(Context context) where TModel:Model, new() {
            return Create(context, typeof(TModel));
        }
        public static IRepository<Context, Model, IController<Context, Model>> Create(Context context, Type modelType) {
            var propertyInfos = context.GetType().GetProperties().Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) && x.PropertyType.GenericTypeArguments.Single() == modelType);
            switch (propertyInfos.Count()) {
                case 0:
                    throw Exception.NotFound($"Unable to locate repository for Model({modelType.Name})");
                case 1:
                    return Create(context, propertyInfos.Single());
                default:
                    throw Exception.BadRequest($"Ambiguous repositories for Model({modelType.Name})");
            }
        }
        public static IRepository<Context, Model, IController<Context, Model>> Create(Context context, string propertyName) {
            var propertyInfos = context.GetType().GetProperties().Where(x => x.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            switch (propertyInfos.Count()) {
                case 0:
                    throw Exception.NotFound($"{propertyName} is not a valid repository name");
                case 1:
                    return Create(context, propertyInfos.Single());
                default:
                    propertyInfos = context.GetType().GetProperties().Where(x => x.Name == propertyName);
                    switch (propertyInfos.Count()) {
                        case 0:
                            throw Exception.NotFound($"{propertyName} is not a valid repository name");
                        case 1:
                            return Create(context, propertyInfos.Single());
                        default:
                            throw Exception.BadRequest($"Repository name({propertyName}) was not unique within the context");
                    }
             }
        }
        public static IRepository<Context, Model, IController<Context, Model>> Create(Context context, PropertyInfo propertyInfo) {
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
            var repository = (IRepository<Context, Model, IController<Context, Model>>)result;
            repository.Name = propertyInfo.Name;
            return repository;


        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RepositoryAttribute : Attribute {
        public RepositoryAttribute(Type type) { Type = type; }
        public Type Type { get; set; }
    }
    public interface IRepository<out TContext, out TModel, out TController> 
        where TContext : Context
        where TModel : Model, new()
        where TController : IController<Context, TModel> {

        TContext Context { get; }
        string Name { get; set; }
        public Type ModelType { get; }
        public Type ControllerType { get; }
    }
    public class Repository<TContext, TModel, TController> : IRepository<TContext, TModel, TController>
        where TContext:Context
        where TModel:Model, new()
        where TController:IController<Context, TModel> {

        public Repository(TContext context) { 
            this.Context = context; 
        }
        public TContext Context { get; }
        public string Name { get; set; } = default!;
        public Type ModelType => typeof(TModel);
        public Type ControllerType => typeof(TController);

        public async Task<TController> FindAsync(object id) {
            var model = await this.Context.Set<TModel>().FindAsync(id);
            if (model == null)
                throw new Exception("Not Found");
            var controller = Activator.CreateInstance(typeof(TController), new object[] { this, model });
            if (controller == null)
                throw new Exception("Unable to create data controller");
            return (TController)controller;
        }

        public IEnumerator<TModel> GetEnumerator() {
            throw new NotImplementedException();
        }

        public TModel Remove(TModel entity) {
            throw new NotImplementedException();
        }
    }
}
