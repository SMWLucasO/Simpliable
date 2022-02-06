using Simpliable.Mapper.Options;

namespace Simpliable.Mapper;

public interface IMapper
{
    void Map<TKey, TValue>(Action<IMappingOptions<TKey, TValue>> rules)
        where TKey : class where TValue : class;

    public TValue? ConvertTo<TKey, TValue>(TKey input)  
        where TKey : class where TValue : class;

    public IList<TValue> ConvertTo<TKey, TValue>(IList<TKey> input) 
        where TKey : class where TValue : class;

}