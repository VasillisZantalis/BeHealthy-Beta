using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class UserRolePrivilegeRepository : GenericRepository<UserRolePrivilege>, IUserRolePrivilegeRepository
{
    public UserRolePrivilegeRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }
}
