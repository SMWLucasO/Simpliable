using Simpliable.Mapper.Builders;
using Simpliable.Mapper.Options;

namespace Simpliable.Mapper;

public class Mapper : IMapper
{
    private Dictionary<Type, IMappingOptions> Mappings { get; set; }
        = new();

    public void Map<TKey, TValue>(Action<IMappingOptions> rules)
    {
        var mappingOptions = new MappingOptions();
        rules.Invoke(mappingOptions);

        Mappings.Add(typeof(TKey), mappingOptions);
    }

    public TValue? ConvertTo<TKey, TValue>(TKey input)
        => MappingConversionBuilder<TKey, TValue>.Get()
            .SetPayload(input)
            .SetCustomOptions(GetMappingOptions<TKey, TValue>())
            .Build().FirstOrDefault();

    public IList<TValue> ConvertTo<TKey, TValue>(IList<TKey> input)
        => MappingConversionBuilder<TKey, TValue>.Get()
            .SetPayload(input)
            .SetCustomOptions(GetMappingOptions<TKey, TValue>())
            .Build();

    private IMappingOptions? GetMappingOptions<TKey, TValue>()
    {
        // TODO: rather than throwing an exception, allow no mappings, some kind of default case.
        var exists = Mappings.TryGetValue(typeof(TKey), out var options);
        if (!exists || options == null)
            return null;

        return options;
    }
}