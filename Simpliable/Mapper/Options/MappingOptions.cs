using System.Linq.Expressions;

namespace Simpliable.Mapper.Options;

public class MappingOptions : IMappingOptions
{
    public Dictionary<string, string> Mappings { get; set; }
        = new();

    public IMappingOptions MapProperty<TKey, TValue, TIn, TOut>(Expression<Func<TKey, TIn>> keyOp,
        Expression<Func<TValue, TOut>> valueOp)
    {
        if (keyOp.Body is not MemberExpression inputMember)
            throw new InvalidOperationException("The keyOp lambda must return a class member variable.");

        if (valueOp.Body is not MemberExpression outputMember)
            throw new InvalidOperationException("The valueOp lambda must return a class member variable.");

        Mappings.Add(inputMember.Member.Name, outputMember.Member.Name);
        return this;
    }
}