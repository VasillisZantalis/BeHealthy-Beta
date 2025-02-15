using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }
}
