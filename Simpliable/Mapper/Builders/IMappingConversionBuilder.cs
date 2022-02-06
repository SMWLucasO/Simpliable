using Simpliable.Mapper.Options;

namespace Simpliable.Mapper.Builders;

public interface IMappingConversionBuilder<TKey, TValue>
{
    public IMappingConversionBuilder<TKey, TValue> SetPayload(IList<TKey> from);
    public IMappingConversionBuilder<TKey, TValue> SetPayload(TKey from);
    
    public IMappingConversionBuilder<TKey, TValue> SetCustomOptions(IMappingOptions options);

    public IList<TValue> Build();

}