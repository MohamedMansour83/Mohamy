using ElMaitre.DTO;
using ElMaitre.Service.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface ILawyerAppointmentService
    {
        IEnumerable<AppointmentDTO> GetAppointments(string userId, int lawyerId);
        IEnumerable<AppointmentGroupedDTO> GetGroupedAppointments(int lawyerId);
        AppointmentDTO GetAppointmentDetails(int Id);
        ResultMessage SetAppointments(IEnumerable<AppointmentDTO> model);
    }
}
