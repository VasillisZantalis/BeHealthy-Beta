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
            .Include(i => i.UserRolePrivileges)
            .ThenInclude(i => i.Role)
            .ToListAsync();

        return privileges;
    }

    public async Task<bool> HasPrivilegeAsync(UserRole role, PrivilegeName privilegeName)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var privilege = await context.Privileges
            .AsQueryable()
            .FirstOrDefaultAsync(w => w.Name == privilegeName);

        if (privilege == null) return false;

        var hasPrivilege = privilege.UserRolePrivileges.Select(s => s.HasPrivilege).FirstOrDefault();

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
                //existingPrivilege.Value = privilege.Value;
                context.Privileges.Update(existingPrivilege);
            }
        }

        await context.SaveChangesAsync();
    }
}
