using System.Linq.Expressions;

namespace Simpliable.Mapper.Options;

public interface IMappingOptions<TKey, TValue>
    where TKey : class where TValue : class
{

    public Dictionary<string, string> Mappings { get; set; }

    public IMappingOptions<TKey, TValue> MapProperty<TIn, TOut>(Expression<Func<TKey, TIn>> keyOp, Expression<Func<TValue, TOut>> valueOp);
}