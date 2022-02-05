using System.Reflection;

namespace Simpliable.DependencyInjection;

public static class Injector
{
    private static InjectionManager Manager { get; set; } = new();

    public static void Map<TAbstraction, TConcretion>() where TConcretion : class
        => Manager.RegisterMapping<TAbstraction, TConcretion>();

    public static TAbstraction Resolve<TAbstraction>()
        => (TAbstraction) Manager.Get(typeof(TAbstraction))!;


    private class InjectionManager
    {
        private Dictionary<Type, Type> Mappings { get; set; }
            = new();

        public void RegisterMapping<TAbstraction, TConcretion>()
            => Mappings.Add(typeof(TAbstraction), typeof(TConcretion));

        public object? Get(Type abstraction)
        {

            // get the relevant constructor.
            Type construction = Mappings[abstraction];
            ConstructorInfo constructor = construction.GetConstructors().First();

            // Just instantiate if there's no arguments.
            if (constructor.GetParameters().Length == 0)
                return InjectPropertiesOf(Activator.CreateInstance(construction));


            // get params since there are some, needed for construction.
            var args = constructor.GetParameters().Select(x =>
                {
                    // if x is of itself, that's a no-no.
                    ThrowOnCyclicConstructorReference(abstraction, x.ParameterType);
                    return Get(x.ParameterType);
                }).ToArray();

            // invoke constructor.
            return InjectPropertiesOf(constructor.Invoke(args));
        }

        private object? InjectPropertiesOf(object? output)
        {
            if (output == null)
                return null;

            output.GetType().GetProperties().ToList()
                .ForEach(x =>
                {
                    // cycles not allowed.
                    ThrowOnCyclicPropertyReference(output, x.PropertyType);

                    output.GetType()
                        .GetProperty(x.Name)?
                        .SetValue(output, Get(x.PropertyType), null);
                });

            return output;
        }

        private static void ThrowOnCyclicPropertyReference(object? output, Type targetType)
        {
            if (output?.GetType().GetInterfaces().Contains(targetType) ?? false)
                throw new InvalidOperationException($"Cyclic property reference detected for {targetType}");
        }

        private static void ThrowOnCyclicConstructorReference(Type abstraction, Type targetType)
        {
            if (abstraction.IsEquivalentTo(targetType))
                throw new InvalidOperationException($"Cyclic constructor reference detected for {targetType}");
        }
    }
}