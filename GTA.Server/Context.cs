using GTA.Data;
using GTA.Data.Models;
using GTA.Server.Controllers;
using GTA.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;

namespace GTA.Server {
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) {
            this.Repositories = new RepositoryCollection(this);
        }
        public Dictionary<string, IEnumerable<string>> RequestHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Person.Configuration());
            modelBuilder.ApplyConfiguration(new User.Configuration());
            modelBuilder.ApplyConfiguration(new Email.Configuration());
            modelBuilder.ApplyConfiguration(new Phone.Configuration());
            modelBuilder.ApplyConfiguration(new PersonEmail.Configuration());
            modelBuilder.ApplyConfiguration(new PersonPhone.Configuration());
            modelBuilder.ApplyConfiguration(new Address.Configuration());
            modelBuilder.ApplyConfiguration(new City.Configuration());
            modelBuilder.ApplyConfiguration(new State.Configuration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public RepositoryCollection Repositories { get; }
        public IRepository<Model, IController<Model>> Repository(string name) {
            return this.Repositories.Get(name);
        }
        public IRepository<Model, IController<Model>> Repository<TModel>() where TModel:Model, new() {
            return this.Repositories.Get<TModel>();
        }

        public LoginController? Login { get; set; }
        public UserController? User => this.Login?.User;

        [Repository(typeof(PersonRepository))] public DbSet<Person> People { get; set; } = default!;
        [Repository(typeof(UserRepository))] public DbSet<User> Users { get; set; } = default!;
        [Repository(typeof(LoginRepository))] public DbSet<Login> Logins { get; set; } = default!;
        [Repository(typeof(CourseRepository))] public DbSet<Course> Courses { get; set; } = default!;

        public DbSet<TeeBox> TeeBoxes { get; set; } = default!;
        public DbSet<Hole> Holes { get; set; } = default!;

        public DbSet<Email> Emails { get; set; } = default!;

        public DbSet<Group> Groups { get; set; } = default!;
        public DbSet<GroupPerson> GroupPeople { get; set; } = default!;
        public DbSet<PersonEmail> PeopleEmails { get; set; } = default!;
        public DbSet<Phone> Phones { get; set; } = default!;
        public DbSet<PersonPhone> PeoplePhones { get; set; } = default!;

        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<City> Cities { get; set; } = default!;
        public DbSet<State> States { get; set; } = default!;



        [Web.Method]
        public object Test() {
            var person = new Person { FirstName = "Steven", LastName = "Okinsky" };
            return this.Repositories.People.Add(person);
            //this.People.Add(person);
            //return this.Repositories.People[person];
        }


    }
    public class RepositoryCollection : IEnumerable<IRepository<Model, IController<Model>>> {
        public RepositoryCollection(Context context) { this.context = context; }
        protected Context context { get; }

        public LoginRepository Logins => (LoginRepository)this.Get<Login>();
        public PersonRepository People => (PersonRepository)this.Get<Person>();



        protected List<IRepository<Model, IController<Model>>> items { get; set; } = new List<IRepository<Model, IController<Model>>>();
        public IEnumerator<IRepository<Model, IController<Model>>> GetEnumerator() {
            return ((IEnumerable<IRepository<Model, IController<Model>>>)items).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)items).GetEnumerator();
        }

        public IRepository<Model, IController<Model>> Create(PropertyInfo propertyInfo) {
            var controllerType = typeof(Controller<>).MakeGenericType(propertyInfo.PropertyType.GenericTypeArguments.Single());
            var controllerAttribute = propertyInfo.GetCustomAttribute<ControllerAttribute>();
            if (controllerAttribute != null)
                controllerType = controllerAttribute.Type;

            var repositoryType = typeof(Repository<,>).MakeGenericType(propertyInfo.PropertyType.GenericTypeArguments.Single(), controllerType);
            var repositoryAttribute = propertyInfo.GetCustomAttribute<RepositoryAttribute>();
            if (repositoryAttribute != null)
                repositoryType = repositoryAttribute.Type;

            var repositoryObject = Activator.CreateInstance(repositoryType, new object[] { this.context });
            if (repositoryObject == null)
                throw Web.Exception.NotFound($"Property({propertyInfo.Name} was unable to create it's repository)");
            var result = (IRepository<Model, IController<Model>>)repositoryObject;
            result.Name = propertyInfo.Name;
            return result;
        }
        public IRepository<Model, IController<Model>> Get(PropertyInfo propertyInfo) {
            var repository = this.items.SingleOrDefault(x => propertyInfo.Name.Equals(x.Name));
            if (repository == null) {
                repository = this.Create(propertyInfo);
                this.items.Add(repository);
            }
            return repository;



        }
        public IRepository<Model, IController<Model>> Get<TModel>() where TModel : Model, new() {
            var propertyInfo = this.context.GetType().GetProperties().SingleOrDefault(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) && typeof(TModel) == x.PropertyType.GenericTypeArguments.SingleOrDefault());
            if (propertyInfo == null)
                throw Web.Exception.NotFound($"Unable to find a repository for ${typeof(TModel).Name}");
            return this.Get(propertyInfo);
        }
        public IRepository<Model, IController<Model>> Get(string name) {
            var propertyInfo = this.context.GetType().GetProperties().SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (propertyInfo == null)
                throw Web.Exception.NotFound($"Unable to find repository with the name '{name}'");
            return this.Get(propertyInfo);
        }

    }
}
