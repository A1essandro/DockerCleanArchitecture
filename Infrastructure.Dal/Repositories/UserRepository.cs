using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories
{

    public class UserRepository : RepositoryBase<User>
    {

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<User> Entities => DbContext.Users.Include(x => x.Sessions);

    }
}