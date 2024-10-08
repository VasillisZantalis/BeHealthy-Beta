using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context)
    {
    }

    //public async Task<Doctor> GetDoctorByUserIdAsync(string id)
    //{
    //    return await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == id);
    //}
}
