using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IServiceService
    {
        IEnumerable<ServiceDTO> Get(int lawyerId = 0);
        ServiceDTO GetById(int Id);
        IEnumerable<ServiceCategoryDTO> GetCategories(int? lawyerId = null);
        IEnumerable<KeyValueDTO> GetCategoriesLookup();
        void AddServiceToLawyer(int ServiceId, int lawyerId, int price, int price2);
        void RemoveServiceLawyer(int ServiceId, int lawyerId);
        void UpdateServiceToLawyer(DAL.Models.LawyerService lservice);
        ServiceDTO GetByLawyerId(int ServiceId, int lawyerId);
        void PayService(int ServiceId, int lawyerId, long orderId);
        bool ConfirmPayment(long orderId);
        void NewOrd(int serviceId, int lawyerId, int orderId, string userId);
        void UpdateOrd(int orderId, string hmac, string created_at,
            string currency, bool is_refund, int txn_response_code, int profile_id,
            bool success, int order, int id, int amount_cents);
    }
}
