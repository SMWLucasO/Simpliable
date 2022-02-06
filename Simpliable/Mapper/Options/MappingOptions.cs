using System.Linq.Expressions;

namespace Simpliable.Mapper.Options;

public class MappingOptions<TKey, TValue> : IMappingOptions<TKey, TValue>
    where TKey : class where TValue : class
{
    public Dictionary<string, string> Mappings { get; set; }
        = new();
    
    public IMappingOptions<TKey, TValue> MapProperty<TIn, TOut>(Expression<Func<TKey, TIn>> keyOp, Expression<Func<TValue, TOut>> valueOp)
    {
        if (keyOp.Body is not MemberExpression inputMember)
            throw new InvalidOperationException("The keyOp lambda must return a class member variable.");

        if (valueOp.Body is not MemberExpression outputMember)
            throw new InvalidOperationException("The valueOp lambda must return a class member variable.");

        return this;
    }
}