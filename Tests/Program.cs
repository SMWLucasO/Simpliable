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

        IMapper mapper = Injector.Resolve<IMapper>();

        mapper.Map<A, B>(options =>
        {
            options.MapProperty<A, B, string, string>(x => x.Name, y => y.Name2)
                .MapProperty<A, B, int, int>(x => x.Id, y => y.Id);
        });

        var l = new List<A>()
        {
            new A {Id = 1, Name = "a"},
            new A {Id = 2, Name = "b"},
            new A {Id = 3, Name = "c"},
            new A {Id = 4, Name = "d"},
            new A {Id = 5, Name = "e"},
            new A {Id = 6, Name = "f"},
        };

        var q = mapper.ConvertTo<A, B>(l);
        foreach (var b in q) Console.WriteLine($"Id={b.Id} Name={b.Name2}");
    }
}