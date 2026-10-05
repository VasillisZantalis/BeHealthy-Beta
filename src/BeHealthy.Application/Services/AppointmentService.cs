using BeHealthy.Application.Common.Extensions;
using BeHealthy.Application.Common.Helpers;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IRoomRepository _roomRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        IRoomRepository roomRepository)
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _roomRepository = roomRepository;
    }

    public async Task<PaginatedResult<AppointmentResponse>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= new();
        var queryOptions = new QueryOptions<Appointment>
        {
            Includes = new List<Expression<Func<Appointment, object>>>
            {
                a => a.Doctor!,
                a => a.Patient!
            },
            PageSize = parameters.PageSize,
            PageNumber = parameters.PageNumber
        };

        Expression<Func<Appointment, bool>> predicate = a => true;

        if (parameters.DoctorId.HasValue && parameters.DoctorId.Value > 0)
        {
            var doctorId = parameters.DoctorId.Value;
            predicate = predicate.And(a => a.DoctorId == doctorId);
        }

        if (parameters.PatientId.HasValue && parameters.PatientId.Value > 0)
        {
            var patientId = parameters.PatientId.Value;
            predicate = predicate.And(a => a.PatientId == patientId);
        }

        queryOptions.Predicate = predicate;

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            queryOptions.OrderBy = OrderByHelper.GetOrderByExpression<Appointment>(parameters.OrderBy);
            queryOptions.OrderDescending = parameters.OrderDescending;
        }

        var appointments = await _appointmentRepository.QueryAsync(queryOptions, cancellationToken);
        var totalCount = await _appointmentRepository.GetCountAsync(cancellationToken);

        return new PaginatedResult<AppointmentResponse>
        {
            Items = appointments.MapToDto(),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId, cancellationToken);
        return appointments.MapToDto();
    }

    public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId, cancellationToken);
        return appointments.MapToDto();
    }

    public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAppointmentsByUserIdAsync(userId, cancellationToken);
        return appointments.MapToDto();
    }

    public async Task<AppointmentResponse?> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        return appointment?.MapToDto();
    }

    public async Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateRequest appointmentDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointment = appointmentDto.MapToDomain();

            var doctorExists = await _doctorRepository.ExistsAsync(appointment.DoctorId, cancellationToken);
            if (!doctorExists)
            {
                return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Doctor));
            }

            var patientExists = await _patientRepository.ExistsAsync(appointment.PatientId, cancellationToken);
            if (!patientExists)
            {
                return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Patient));
            }

            if (appointment.RoomId.HasValue
                && !await _roomRepository.ExistsAsync(appointment.RoomId.Value, cancellationToken))
            {
                return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Room));
            }

            var conflictCheck = await CheckForConflictingAppointmentsAsync(
                appointmentDto.DoctorId,
                appointmentDto.PatientId,
                appointmentDto.NurseId,
                appointmentDto.RoomId,
                appointmentDto.AppointmentDate,
                appointmentDto.AppointmentStartTime,
                appointmentDto.AppointmentEndTime,
                cancellationToken: cancellationToken);

            if (!conflictCheck.Success)
            {
                return conflictCheck;
            }

            await _appointmentRepository.AddAsync(appointment, cancellationToken);
            await _appointmentRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }

    public async Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateRequest appointmentDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentDto.Id, cancellationToken);
            if (appointment is null)
            {
                return ServiceResponse.Failed(Resource.NotFound);
            }

            var doctorExists = await _doctorRepository.ExistsAsync(appointmentDto.DoctorId, cancellationToken);
            if (!doctorExists)
            {
                return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Doctor));
            }

            var patientExists = await _patientRepository.ExistsAsync(appointmentDto.PatientId, cancellationToken);
            if (!patientExists)
            {
                return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Patient));
            }

            if (appointmentDto.RoomId.HasValue
                && !await _roomRepository.ExistsAsync(appointmentDto.RoomId.Value, cancellationToken))
            {
                return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Room));
            }

            var conflictCheck = await CheckForConflictingAppointmentsAsync(
                appointmentDto.DoctorId,
                appointmentDto.PatientId,
                appointmentDto.NurseId,
                appointmentDto.RoomId,
                appointmentDto.AppointmentDate,
                appointmentDto.AppointmentStartTime,
                appointmentDto.AppointmentEndTime,
                appointmentDto.Id,
                cancellationToken);

            if (!conflictCheck.Success)
            {
                return conflictCheck;
            }

            appointment.PatientId = appointmentDto.PatientId;
            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.AppointmentStartTime = appointmentDto.AppointmentStartTime;
            appointment.AppointmentEndTime = appointmentDto.AppointmentEndTime;
            appointment.Notes = appointmentDto.Notes;
            appointment.Status = appointmentDto.Status;
            appointment.Reason = appointmentDto.Reason;
            appointment.RoomId = appointmentDto.RoomId;
            appointment.NurseId = appointmentDto.NurseId;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }

    public async Task DeleteAppointmentAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _appointmentRepository.DeleteAsync(id, cancellationToken))
        {
            await _appointmentRepository.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts(CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);

        var groupedByReason = appointments
            .GroupBy(x => x.Reason)
            .Select(x => new
            {
                x.Key,
                Count = x.Count()
            })
            .ToDictionary(k => k.Key, v => v.Count);

        return groupedByReason;
    }

    private async Task<ServiceResponse> CheckForConflictingAppointmentsAsync(
        int doctorId,
        int patientId,
        int? nurseId,
        int? roomId,
        DateOnly appointmentDate,
        TimeOnly appointmentStartTime,
        TimeOnly appointmentEndTime,
        int? appointmentId = null,
        CancellationToken cancellationToken = default)
    {
        DateTime newStart = appointmentDate.ToDateTime(appointmentStartTime);
        DateTime newEnd = appointmentDate.ToDateTime(appointmentEndTime);

        var doctorAppointments = await _appointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctorId, cancellationToken);
        var doctorConflict = FindConflict(doctorAppointments, newStart, newEnd, appointmentId);

        if (doctorConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForDoctor,
                doctorConflict.Doctor?.FullName,
                doctorConflict.AppointmentStartTime.ToShortTimeString(),
                doctorConflict.AppointmentEndTime.ToShortTimeString()
            );
            return ServiceResponse.Failed(errorMessage);
        }

        var patientAppointments = await _appointmentRepository.GetAllAppointmentsByPatientIdAsync(patientId, cancellationToken);
        var patientConflict = FindConflict(patientAppointments, newStart, newEnd, appointmentId);

        if (patientConflict != null)
        {
            var errorMessage = string.Format(
                Resource.AppointmentExistsForPatient,
                patientConflict.Patient?.FullName,
                patientConflict.AppointmentStartTime.ToShortTimeString(),
                patientConflict.AppointmentEndTime.ToShortTimeString()
            );
            return ServiceResponse.Failed(errorMessage);
        }

        if (nurseId.HasValue)
        {
            var nurseAppointments = await _appointmentRepository.GetAllAppointmentsByNurseIdAsync(nurseId.Value, cancellationToken);
            var nurseConflict = FindConflict(nurseAppointments, newStart, newEnd, appointmentId);

            if (nurseConflict != null)
            {
                var errorMessage = string.Format(
                    Resource.AppointmentExistsForNurse,
                    nurseConflict.Nurse?.FullName,
                    nurseConflict.AppointmentStartTime.ToShortTimeString(),
                    nurseConflict.AppointmentEndTime.ToShortTimeString()
                );
                return ServiceResponse.Failed(errorMessage);
            }
        }

        if (roomId.HasValue)
        {
            var roomAppointments = await _roomRepository.GetRoomAppointmentsAsync(roomId.Value, cancellationToken);
            var roomConflict = FindConflict(roomAppointments, newStart, newEnd, appointmentId);

            if (roomConflict != null)
            {
                return ServiceResponse.Failed(Resource.RoomIsBookedAtThatTime);
            }
        }

        return ServiceResponse.Successful();
    }

    private Appointment? FindConflict(IEnumerable<Appointment> appointments, DateTime newStart, DateTime newEnd, int? excludeId)
    {
        return appointments.FirstOrDefault(existing =>
        {
            if (excludeId.HasValue && existing.Id == excludeId.Value)
            {
                return false;
            }

            DateTime existingStart = existing.AppointmentDate.ToDateTime(existing.AppointmentStartTime);
            DateTime existingEnd = existing.AppointmentDate.ToDateTime(existing.AppointmentEndTime);

            return newStart <= existingEnd && newEnd >= existingStart;
        });
    }

    public async Task<IEnumerable<AppointmentResponse>> GetUpcomingAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var threeDaysFromNow = today.AddDays(3);


        QueryOptions<Appointment> queryOptions = new QueryOptions<Appointment>
        {
            Predicate = a => a.AppointmentDate >= today
                            && a.AppointmentDate <= threeDaysFromNow
                            && a.Status != AppointmentStatus.Cancelled
                            && a.Status != AppointmentStatus.Completed,
            OrderBy = a => a.AppointmentDate,
            PageSize = 5
        };

        var appointments = await _appointmentRepository.QueryAsync(queryOptions, cancellationToken);

        return appointments.MapToDto();
    }
}

