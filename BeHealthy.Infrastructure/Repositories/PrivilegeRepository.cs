using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeHealthy.Infrastructure.Repositories;

public class PrivilegeRepository : GenericRepository<Privilege>, IPrivilegeRepository
{
    public PrivilegeRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<Dictionary<string, bool>> GetUserPrivilegesAsync(UserRole userRole)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var privileges = await context.Privileges
            .AsQueryable()
            .Include(p => p.RolePrivileges)
            .Where(p => p.RolePrivileges.Any(rp => rp.Role == userRole))
            .Select(p => new
            {
                Name = p.Name!,
                p.Value
            })
            .ToDictionaryAsync(k => k.Name, v => v.Value);

        return privileges;
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
            .Any(rp => rp.Role == role && rp.Privilege!.Value);

        return hasPrivilege;
    }
}
