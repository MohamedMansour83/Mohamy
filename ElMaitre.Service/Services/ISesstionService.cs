using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface ISesstionService
    {
        IEnumerable<SesstionDTO> GetLawyerSesstions(int Id);
        IEnumerable<SesstionDTO> GetSesstions(string UserId);
		IEnumerable<SesstionDTO> GetPendingLiveSesstions(string UserId);
		IEnumerable<SesstionDTO> GetLawyerPendingLiveSesstions(int LawyerId);
		void Add(SesstionDTO sesstion);
        void Update(SesstionDTO sesstion);
        void UpdateX(SesstionDTO sesstion);

        SesstionDTO GetSesstionById(string id);
		SesstionDTO GetSesstionByAppointmentId(int id);
        SesstionDTO GetSesstionBymerchantId(int id);
        SesstionDTO GetSesstionByOrderId(string id);
        void RateSession(string sessionId, string userId, int rate, string review);
        SesstionDTO EndSession(string id);
        void InsertNote(SessionNoteDTO note);
        bool ConfirmPayment(string orderId);
    }
}
