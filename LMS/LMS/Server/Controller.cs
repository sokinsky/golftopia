namespace LMS.Data.Server {
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute {
        public ControllerAttribute(Type type) { Type = type; }
        public Type Type { get; set; }
    }
    public interface IController<out TContext, out TModel>
        where TContext:Context
        where TModel:Model, new() {

        TContext Context { get; }
        TModel Model { get; }

    }
    public class Controller<TContext, TModel> : IController<TContext, TModel>
        where TContext:Context
        where TModel:Model, new() {
        public Controller(TContext context, TModel model) { this.Context = context; this.Model = model; }
        public TContext Context { get; }
        public TModel Model { get; }
    }
}
