using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using ElMaitre.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ElMaitre.Services
{
    public class ServiceService : IServiceService
    {
        private IRepository<DAL.Models.Service> ServiceRepository;
        private IRepository<PaymentService> PaymentServiceRepository;
        private IRepository<ServiceCategory> ServiceCategoryRepository;
        private IRepository<Order> OrderRepository;
        private IRepository<DAL.Models.LawyerService> lawyerServiceRepository;
        private IRepository<Lawyer> lawyerRepository;

        public ServiceService(IRepository<DAL.Models.Service> ServiceRepository, IRepository<ServiceCategory> ServiceCategoryRepository,
            IRepository<DAL.Models.LawyerService> lawyerServiceRepository, IRepository<Lawyer> lawyerRepository,
            IRepository<PaymentService> PaymentServiceRepository, IRepository<Order> OrderRepository)
        {
            this.ServiceRepository = ServiceRepository;
            this.OrderRepository = OrderRepository;
            this.lawyerServiceRepository = lawyerServiceRepository;
            this.lawyerRepository = lawyerRepository;
            this.PaymentServiceRepository = PaymentServiceRepository;
            this.ServiceCategoryRepository = ServiceCategoryRepository;
        }

        public IEnumerable<ServiceDTO> Get(int lawyerId = 0)
        {
            Expression<Func<DAL.Models.Service, bool>> filter = s => s.Id > 0;

            if (lawyerId > 0)
            {

                filter = s => s.Lawyers.Any(a => a.LawyerId == lawyerId);
            }
            var lawyer = lawyerRepository.Get(l => l.Id == lawyerId, null, "Services").FirstOrDefault();
            var lst = ServiceRepository.Get(filter, includeProperties: "Prices").Select(s => ServiceDTO.ToServiceDTO(s, lawyer)).ToList();
            return lst;
        }

        public ServiceDTO GetById(int Id)
        {
            return ServiceDTO.ToServiceDTO(ServiceRepository.Get(Id));
        }

        public IEnumerable<ServiceCategoryDTO> GetCategories(int? lawyerId = null)
        {
            var lawyer = lawyerRepository.Get(l => l.Id == lawyerId, null, "Services").FirstOrDefault();
            return ServiceCategoryRepository.
                Get(includeProperties: "Services").Select(s => ServiceCategoryDTO.ToServiceCategoryDTO(s, lawyer));
        }

        public IEnumerable<KeyValueDTO> GetCategoriesLookup()
        {
            return ServiceCategoryRepository.Get().Select(s => new KeyValueDTO { Id = s.Id, Value = s.Name, ValueEn = s.NameEn });
        }


        public void AddServiceToLawyer(int ServiceId, int lawyerId, int price, int price2)
        {
            lawyerServiceRepository.Insert(new DAL.Models.LawyerService
            {
                LawyerId = lawyerId,
                ServiceId = ServiceId,
                PriceProvided = price,
                PriceLevel2Provided = price2
            });
        }

        public void UpdateServiceToLawyer(DAL.Models.LawyerService lservice)
        {
            try
            {
                lawyerServiceRepository.Update(lservice);
            }
            catch (Exception ex)
            {

            }
        }

        public void RemoveServiceLawyer(int ServiceId, int lawyerId)
        {
            var row = lawyerServiceRepository.Get(s => s.LawyerId == lawyerId && s.ServiceId == ServiceId).FirstOrDefault();
            if (row!=null)
            {
                lawyerServiceRepository.Delete(row);
            }
        }

        public ServiceDTO GetByLawyerId(int ServiceId, int lawyerId)
        {
            var lawyer = lawyerRepository.Get(s => s.Id == lawyerId, null, "Services").FirstOrDefault();
            var service = ServiceRepository.Get(s => s.Id == ServiceId, includeProperties: "Prices,Lawyers").FirstOrDefault();
            if (lawyer != null && service != null)
            {
                var serv = ServiceDTO.ToServiceDTO(service, lawyer);
                return serv;
            }

            return null;
        }

        //public int GetServiceCategory(int ServiceId)
        //{
        //    var service = ServiceRepository.Get(s => s.Id == ServiceId, includeProperties: "Prices,Lawyers").FirstOrDefault();
        //    if (service != null)
        //    {
        //        var serv = ServiceDTO.ToServiceDTO(service, lawyer);
        //        return serv;
        //    }

        //    return null;
        //}


        public void PayService(int ServiceId, int lawyerId, long orderId)
        {
            var p = PaymentServiceRepository.Get(s => s.LawyerId == lawyerId && s.ServiceId == ServiceId).FirstOrDefault();
            if (p == null)
            {
                PaymentServiceRepository.Insert(new PaymentService { LawyerId = lawyerId, ServiceId = ServiceId, OrderId = orderId });
            }
        }

        public void NewOrd(int serviceId, int lawyerId, int orderId, string userId)
        {
            var o = OrderRepository.Get(s => s.lawyerId == lawyerId 
            && s.serviceId == serviceId && s.merchant_order_id == orderId
            && s.clientId == userId).FirstOrDefault();
            if (o == null)
            {
                OrderRepository.Insert(new Order
                {
                    serviceId = serviceId,
                    lawyerId = lawyerId,
                    merchant_order_id = orderId,
                    clientId = userId
                });
            }
        }

        public void UpdateOrd(int orderId, string hmac, string created_at,
            string currency, bool is_refund, int txn_response_code, int profile_id,
            bool success, int order, int id, int amount_cents)
        {
            var o = OrderRepository.Get(s => s.merchant_order_id == orderId).FirstOrDefault();
            if (o != null)
            {
                o.hmac = hmac;
                o.created_at = created_at;
                o.currency = currency;
                o.is_refund = is_refund;
                o.txn_response_code = txn_response_code;
                o.profile_id = profile_id;
                o.success = success;
                o.order = order;
                o.ordtrans_id = id;
                o.amount_cents = amount_cents;
                OrderRepository.Update(o);
            }
        }

        public bool ConfirmPayment(long orderId)
        {
            var p = PaymentServiceRepository.Get(s => s.OrderId == orderId).FirstOrDefault();
            if (p == null)
            {
                p.IsPaymentConfirmed = true;
                return true;
            }

            return false;
        }
    }
}
