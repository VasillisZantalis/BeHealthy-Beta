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

    public async Task<List<Privilege>> GetPrivilegesAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var privileges = await context.Privileges
            .AsQueryable()
            .ToListAsync();

        return privileges;
    }

    public async Task<Dictionary<PrivilegeName, bool>> GetUserPrivilegesAsync(UserRole userRole)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var privileges = await context.Privileges
            .AsQueryable()
            .Where(p => p.Role == userRole)
            .Select(p => new
            {
                Name = p.Name!,
                p.Value
            })
            .ToDictionaryAsync(k => k.Name, v => v.Value);

        return privileges;
    }

    public async Task<bool> HasPrivilegeAsync(UserRole role, PrivilegeName privilegeName)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var privilege = await context.Privileges
            .AsQueryable()
            .FirstOrDefaultAsync(w => w.Name == privilegeName);

        if (privilege == null) return false;

        var hasPrivilege = privilege.Value;

        return hasPrivilege;
    }

    public async Task UpdatePrivilegesAsync(List<Privilege> privileges)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        foreach (var privilege in privileges)
        {
            var existingPrivilege = await context.Privileges.FindAsync(privilege.Id);
            if (existingPrivilege != null)
            {
                existingPrivilege.Value = privilege.Value;
                context.Privileges.Update(existingPrivilege);
            }
        }

        await context.SaveChangesAsync();
    }
}
