using Simpliable.Mapper.Options;

namespace Simpliable.Mapper.Builders;

public interface IMappingConversionBuilder<TKey, TValue> where TKey : class where TValue : class
{
    public IMappingConversionBuilder<TKey, TValue> SetPayload(IList<TKey> from);
    public IMappingConversionBuilder<TKey, TValue> SetPayload(TKey from);
    
    public IMappingConversionBuilder<TKey, TValue> SetCustomOptions(IMappingOptions<TKey, TValue> options);

    public IMappingConversionBuilder<TKey, TValue> BuildDefaultMappings();
    public IMappingConversionBuilder<TKey, TValue> BuildCustomizedMappings();

    public IList<TValue> Build();

}