namespace Noodle_Assignment_API.Services
{
    public interface IHelloWorldService
    {
        public string HelloWorld();
    }
    public class HelloWorldService : IHelloWorldService
    {
        public string HelloWorld()
        {
            return "Hello world DI";
        }
    }
}
