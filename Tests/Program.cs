// See https://aka.ms/new-console-template for more information

using Simpliable.DependencyInjection;

internal interface IAnother
{
    public string GetPassword();
}

internal class Another : IAnother
{
    public string GetPassword()
        => "Hello there!";
}

internal interface IExample
{
    public IAnother Another { get; set; }
    
    public IExample Examp { get; set; }
    
    public string GetName();
}

internal class Example : IExample
{

    // Cyclic references are not allowed.
    public IAnother Another { get; set; }
    
    public IExample Examp { get; set; }

    public string GetName()
        => "This was an example.";
}

public class Program
{
    public static void Main(string[] args)
    {
        Injector.Map<IExample, Example>();
        Injector.Map<IAnother, Another>();

        Console.WriteLine(Injector.Resolve<IExample>()?
                .Another
                .GetPassword());
    }
}