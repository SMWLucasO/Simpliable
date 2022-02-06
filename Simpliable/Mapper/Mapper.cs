using Simpliable.Mapper.Builders;
using Simpliable.Mapper.Options;

namespace Simpliable.Mapper;

public class Mapper : IMapper
{
    private Dictionary<Type, IMappingOptions<Type, Type>> Mappings { get; set; }
        = new();

    public void Map<TKey, TValue>(Action<IMappingOptions<TKey, TValue>> rules)
        where TKey : class where TValue : class
    {
        IMappingOptions<TKey, TValue> mappingOptions = new MappingOptions<TKey, TValue>();
        rules.Invoke(mappingOptions);

        Mappings.Add(typeof(TKey), (IMappingOptions<Type, Type>) mappingOptions);
    }

    public TValue? ConvertTo<TKey, TValue>(TKey input) where TKey : class where TValue : class =>
        MappingConversionBuilder<TKey, TValue>.For<TKey, TValue>()
            .SetPayload(input)
            .SetCustomOptions((IMappingOptions<TKey, TValue>) GetMappingOptions<TKey, TValue>())
            .Build().FirstOrDefault();

    public IList<TValue> ConvertTo<TKey, TValue>(IList<TKey> input) where TKey : class where TValue : class =>
        MappingConversionBuilder<TKey, TValue>.For<TKey, TValue>()
            .SetPayload(input)
            .SetCustomOptions((IMappingOptions<TKey, TValue>) GetMappingOptions<TKey, TValue>())
            .Build();

    private IMappingOptions<Type, Type> GetMappingOptions<TKey, TValue>() where TKey : class where TValue : class
    {
        var exists = Mappings.TryGetValue(typeof(TKey), out var options);
        if (!exists || options == null)
            throw new InvalidOperationException(
                $"Tried converting {typeof(TKey)} to {typeof(TValue)}, but no mappings were found.");

        return options;
    }
}