using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LMS.Data.Server {
    public class Context : DbContext {
        public Context(DbContextOptions options) : base(options) {

        }
        public Dictionary<string, IEnumerable<string>> RequestHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseLazyLoadingProxies();
        }


        protected List<IRepository<Context, Model, IController<Context, Model>>> Repositories { get; set; } = new List<IRepository<Context, Model, IController<Context, Model>>>();
        public IRepository<Context, Model, IController<Context, Model>> Repository(string name) {
            var result = this.Repositories.SingleOrDefault(x => x.Name == name);
            if (result != null)
                return result;

            var property = this.GetType().GetProperties().SingleOrDefault(x => x.Name == name);
            if (property == null)
                property = this.GetType().GetProperties().SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (property == null)
                throw new System.Exception($"Property({name}) Not Found");

            var repositoryAttribute = property.GetCustomAttribute<RepositoryAttribute>();
            if (repositoryAttribute == null)
                throw new System.Exception($"Property({name} has not been assigned a repository");

            var repositoryObject = Activator.CreateInstance(repositoryAttribute.Type, new object[] { this });
            if (repositoryObject == null)
                throw new System.Exception($"Property({name} was unable to create it's repository");
            var repository = (IRepository<Context, Model, IController<Context, Model>>)repositoryObject;
            repository.Name = property.Name;
            this.Repositories.Add(repository);
            return repository;
        }


    }
}
