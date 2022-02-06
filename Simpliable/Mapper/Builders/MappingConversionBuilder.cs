using Simpliable.Mapper.Options;

namespace Simpliable.Mapper.Builders;

public class MappingConversionBuilder<TKey, TValue> : IMappingConversionBuilder<TKey, TValue>
    where TKey : class where TValue : class
{
    public IList<TKey> Payload { get; set; }

    
    public IList<TValue> Output { get; set; }
        = new List<TValue>();

    public IMappingOptions<TKey, TValue> CustomizedMappings { get; set; }

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

    public IMappingConversionBuilder<TKey, TValue> SetCustomOptions(IMappingOptions<TKey, TValue> options)
    {
        this.CustomizedMappings = options;
        return this;
    }

    public IMappingConversionBuilder<TKey, TValue> BuildDefaultMappings()
    {
        throw new NotImplementedException();
    }

    public IMappingConversionBuilder<TKey, TValue> BuildCustomizedMappings()
    {
        throw new NotImplementedException();
    }

    public IList<TValue> Build()
    {
        this.BuildDefaultMappings().BuildCustomizedMappings();

        return this.Output;
    } 

    public static IMappingConversionBuilder<TKey, TValue> For<TKey, TValue>() where TValue : class where TKey : class
        => new MappingConversionBuilder<TKey, TValue>();
}