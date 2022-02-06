// See https://aka.ms/new-console-template for more information

using Simpliable.DependencyInjection;
using Simpliable.Mapper;

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
    
    //public IExample Examp { get; set; }
    
    public string GetName();
}

internal class Example : IExample
{

    // Cyclic references are not allowed.
    public IAnother Another { get; set; }
    
    //public IExample Examp { get; set; }

    public string GetName()
        => "This was an example.";
}

class A
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class B
{
    public int Id { get; set; }
    public string Name2 { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        Injector.Map<IExample, Example>();
        Injector.Map<IAnother, Another>();

        Injector.Map<IMapper, Mapper>();
        
        Console.WriteLine(Injector.Resolve<IExample>()?
                .Another
                .GetPassword());
        
        Injector.Resolve<IMapper>().Map<A, B>(options =>
        {
            options.MapProperty(x => x.Name, y => y.Name2)
                .MapProperty(x => x.Id, y => y.Id);
        });
        
        
        
    }
}