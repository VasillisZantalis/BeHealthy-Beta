using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class PrivilegeRepository : GenericRepository<Privilege>, IPrivilegeRepository
{
    public PrivilegeRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<bool> HasPrivilegeAsync(UserRole role, string privilegeName)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var privilege = await context.Privileges
            .AsQueryable()
            .Include(p => p.RolePrivileges)
            .FirstOrDefaultAsync(w => w.Name == privilegeName);

        if (privilege == null) return false;

        var hasPrivilege = privilege.RolePrivileges
            .Any(rp => rp.Role == role && rp.Privilege.Value);

        return hasPrivilege;
    }
}
