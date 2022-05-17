using System.Linq.Expressions;
using Core.Domain;

namespace Core.Specifications;

public class ByEmailSpec : Specification<User>
{

    private readonly string _email;

    public ByEmailSpec(string email)
    {
        _email = email.Trim().ToUpper();
    }

    public override Expression<Func<User, bool>> ToExpression() => x => string.Equals(x.Email.ToUpper(), _email);

}