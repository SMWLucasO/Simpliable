using System.Reflection;

namespace Simpliable.DependencyInjection;

public static class Injector
{
    private static InjectionManager Manager { get; set; } = new();

    public static void Map<TAbstraction, TConcretion>() where TConcretion : class, new()
        => Manager.RegisterMapping<TAbstraction, TConcretion>();

    public static TAbstraction Resolve<TAbstraction>()
        => (TAbstraction) Manager.Get(typeof(TAbstraction), new Dictionary<Type, object?>())!;


    private class InjectionManager
    {
        private Dictionary<Type, Type> Mappings { get; set; }
            = new();

        public void RegisterMapping<TAbstraction, TConcretion>()
            => Mappings.Add(typeof(TAbstraction), typeof(TConcretion));

        public object? Get(Type abstraction, Dictionary<Type, object?> cache = default!)
        {
            if (cache.ContainsKey(abstraction))
                return cache[abstraction];

            // get the relevant constructor.
            Type construction = Mappings[abstraction];
            ConstructorInfo constructor = construction.GetConstructors().First();

            // Just instantiate if there's no arguments.
            if (constructor.GetParameters().Length == 0)
            {
                cache[abstraction] = Activator.CreateInstance(construction);
                return InjectPropertiesOf(cache[abstraction], cache);
            }


            // get params since there are some, needed for construction.
            var args = constructor.GetParameters()
                .Select(x => Get(x.ParameterType, cache))
                .ToArray();

            // invoke constructor.
            return InjectPropertiesOf(constructor.Invoke(args), cache);
        }

        private object? InjectPropertiesOf(object? output, Dictionary<Type, object?> cache)
        {
            if (output == null)
                return null;

            output.GetType().GetProperties().ToList()
                .ForEach(x =>
                {
                    output.GetType()
                        .GetProperty(x.Name)?
                        .SetValue(output, Get(x.PropertyType, cache), null);
                });

            return output;
        }
    }
}