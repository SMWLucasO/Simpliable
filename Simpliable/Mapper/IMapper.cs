using Simpliable.Mapper.Options;

namespace Simpliable.Mapper;

public interface IMapper
{
    void Map<TKey, TValue>(Action<IMappingOptions> rules);

    public TValue? ConvertTo<TKey, TValue>(TKey input);

    public IList<TValue> ConvertTo<TKey, TValue>(IList<TKey> input);

}