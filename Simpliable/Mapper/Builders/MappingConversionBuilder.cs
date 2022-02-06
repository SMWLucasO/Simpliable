using System.Collections.Immutable;
using Simpliable.Mapper.Options;

namespace Simpliable.Mapper.Builders;

public class MappingConversionBuilder<TKey, TValue> : IMappingConversionBuilder<TKey, TValue>
{
    private IList<TKey> Payload { get; set; }
    
    private IList<TValue> Output { get; set; }
        = new List<TValue>();

    public IMappingOptions? CustomizedMappings { get; set; }

    public IMappingConversionBuilder<TKey, TValue> SetPayload(IList<TKey> from)
    {
        this.Payload = from;
        return this;
    }

    public IMappingConversionBuilder<TKey, TValue> SetPayload(TKey from)
    {
        this.Payload = new List<TKey> {from};
        return this;
    }

    public IMappingConversionBuilder<TKey, TValue> SetCustomOptions(IMappingOptions options)
    {
        this.CustomizedMappings = options;
        return this;
    }

    private MappingConversionBuilder<TKey, TValue> BuildDefaultMappings()
    {
        var availableDefaultMappings = typeof(TValue).GetProperties()
            .Select(x => x.Name)
            .ToImmutableHashSet()
            .Intersect(
                typeof(TKey).GetProperties()
                    .Select(x => x.Name)
                    .ToImmutableHashSet()
            );

        // Set default mappings, e.g. Name => Name, Id => Id.
        // Possibly add a rule system, e.g. ignore case.
        foreach (var key in this.Payload)
        {
            var obj = typeof(TValue).GetConstructors().First().Invoke(null);

            foreach (var @default in availableDefaultMappings)
                obj.GetType().GetProperty(@default)?
                    .SetValue(obj, key?.GetType().GetProperty(@default)?.GetValue(key));

            Output.Add((TValue) obj);
        }

        return this;
    }

    private void BuildCustomizedMappings()
    {
        if (CustomizedMappings is not {Mappings.Count: not 0})
            return;

        for (int i = 0; i < Output.Count; i++)
            foreach (var (from, to) in CustomizedMappings.Mappings)
                Output[i]?.GetType().GetProperty(to)?.SetValue(Output[i],
                    Payload[i]?.GetType().GetProperty(from)?.GetValue(Payload[i]));
    }

    public IList<TValue> Build()
    {
        this.BuildDefaultMappings().BuildCustomizedMappings();
        return this.Output;
    }

    public static IMappingConversionBuilder<TKey, TValue> Get()
        => new MappingConversionBuilder<TKey, TValue>();
}