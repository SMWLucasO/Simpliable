using System.Linq.Expressions;

namespace Simpliable.Mapper.Options;

public interface IMappingOptions
{
    public Dictionary<string, string> Mappings { get; set; }

    public IMappingOptions MapProperty<TKey, TValue, TIn, TOut>(Expression<Func<TKey, TIn>> keyOp,
        Expression<Func<TValue, TOut>> valueOp);
}