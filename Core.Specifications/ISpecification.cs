using System.Linq.Expressions;

namespace Core.Specifications;

public interface ISpecification<T>
{

    Expression<Func<T, bool>> ToExpression();

    bool IsSatisfiedBy(T entity);

}
