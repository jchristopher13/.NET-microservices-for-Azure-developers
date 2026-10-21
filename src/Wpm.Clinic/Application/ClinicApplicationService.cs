using Wpm.Clinic.DataAccess;
using Wpm.Clinic.ExternalServices;
using Wpm.Clinic.Controllers;

namespace Wpm.Clinic.Application;

public class ClinicApplicationService(ClinicDbContext dbContext,
    ManagementService managementService)
{
    public async Task<Consultation> Handle(StartConsultationCommand command)
    {
        var petInfo = await managementService.GetPetInfoAsync(command.PatientId);

        var newConsultation = new Consultation(Guid.NewGuid(),
            command.PatientId,
            petInfo.Name,
            petInfo.Age,
            DateTime.UtcNow);

        await dbContext.Consultations.AddAsync(newConsultation);
        await dbContext.SaveChangesAsync();

        return newConsultation;
    }
}
