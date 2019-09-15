using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using ElMaitre.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElMaitre.Services
{
    public class SesstionService : ISesstionService
    {
        private IRepository<Session> SesstionRepository;
        private ILawyerAppointmentService LawyerAppointmentService;
        private IRepository<Lawyer> LawyerRepository ;
        private IRepository<SessionNote> SessionNoteRepository;

        public SesstionService(IRepository<Session> SesstionRepository, ILawyerAppointmentService LawyerAppointmentService, IRepository<Lawyer> LawyerRepository,
            IRepository<SessionNote> SessionNoteRepository)
        {
            this.SesstionRepository = SesstionRepository;
            this.LawyerAppointmentService = LawyerAppointmentService;
            this.LawyerRepository = LawyerRepository;
            this.SessionNoteRepository = SessionNoteRepository;
        }

        public IEnumerable<SesstionDTO> GetSesstions(string UserId)
        {
            var res = SesstionRepository.Get(s => s.UserId == UserId && s.Appointment.Date>=DateTime.Now.ToEgyptTimezone().Date, includeProperties: "Appointment,Appointment.Lawyer.User").OrderByDescending(s => s.Appointment.Date.Add(s.Appointment.Time));
            var result = res.Select(s => new SesstionDTO
            {
                Id = s.Id,
                SessionId = s.SessionId,
                UserId = s.UserId,
                StartedDate = s.StartedDate,
                Duration = s.Duration,
                AppointmentId = s.AppointmentId,
                Appointment = new AppointmentDTO
                {
                    Id = s.Appointment.Id,
                    Date = s.Appointment.Date,
                    DateTxt = s.Appointment.Date.ToString("dddd, dd MMMM yyyy"),
                    LawyerId = s.Appointment.LawyerId,
                    Time = s.Appointment.Time,
                    TimeFrom = new DateTime().Add(s.Appointment.Time).ToString("hh:mm tt"),
                    TimeTo = new DateTime().Add(s.Appointment.Time.Add(new TimeSpan(0, s.Duration, 0))).ToString("hh:mm tt"),
                    LawyerName = s.Appointment.Lawyer.User.Name,
                    LawyerNameEn = s.Appointment.Lawyer.User.NameEn,
                    Amount = s.Appointment.Lawyer.Fees.HasValue ? s.Appointment.Lawyer.Fees.Value : 0,
                    Discount = 0
                }
            });



            return result;
        }

        public IEnumerable<SesstionDTO> GetLawyerSesstions(int Id)
        {
            var res = SesstionRepository.Get(s => s.Appointment.LawyerId == Id && s.Appointment.Date >= DateTime.Now.ToEgyptTimezone().Date, includeProperties: "Appointment,Appointment.Lawyer.User,Notes,Notes.User").OrderByDescending(s => s.Appointment.Date.Add(s.Appointment.Time));

            var result = res.Select(s => new SesstionDTO
            {
                Id = s.Id,
                SessionId = s.SessionId,
                UserId = s.UserId,
                StartedDate = s.StartedDate,
                Duration = s.Duration,
                AppointmentId = s.AppointmentId,
                Appointment = new AppointmentDTO
                {
                    Id = s.Appointment.Id,
                    Date = s.Appointment.Date,
                    DateTxt = s.Appointment.Date.ToString("dddd, dd MMMM yyyy"),
                    LawyerId = s.Appointment.LawyerId,
                    Time = s.Appointment.Time,
                    TimeFrom = new DateTime().Add(s.Appointment.Time).ToString("hh:mm tt"),
                    TimeTo = new DateTime().Add(s.Appointment.Time.Add(new TimeSpan(0, s.Duration, 0))).ToString("hh:mm tt"),
                    LawyerName = s.Appointment.Lawyer.User.Name,
                    LawyerNameEn = s.Appointment.Lawyer.User.NameEn,
                    Amount = s.Appointment.Lawyer.Fees.HasValue ? s.Appointment.Lawyer.Fees.Value : 0,
                    Discount = 0
                },
                Notes = s.Notes.Select(n => new SessionNoteDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    UserId = n.UserId,
                    SessionId = n.SessionId,
                    UserName = n.User.Name,
                    UserNameEn = n.User.NameEn
                })
            });
            return result;
        }


        public void Add(SesstionDTO sesstion)
        {
			SesstionRepository.Insert(new Session
			{
				UserId = sesstion.UserId,
				StartedDate = sesstion.StartedDate,
				Duration = sesstion.Duration,
				AppointmentId = sesstion.AppointmentId,
				OrderId = sesstion.OrderId,
				merchant_order_id = sesstion.merchant_order_id
			});

        }

        public bool ConfirmPayment(string orderId)
        {
            var sesstion = SesstionRepository.Get(s=>s.OrderId== orderId && !s.IsPaymentConfirmed && !s.IsFinished).FirstOrDefault();
            if (sesstion!=null)
            {
                sesstion.IsPaymentConfirmed = true;
                SesstionRepository.Update(sesstion);
                return true;
            }

            return false;

        }
        public void Update(SesstionDTO sesstion)
        {
            SesstionRepository.Update(new Session
            {
                Id = sesstion.Id,
                SessionId = sesstion.SessionId,
                UserId = sesstion.UserId,
                StartedDate = sesstion.StartedDate,
                Duration = sesstion.Duration,
                AppointmentId = sesstion.AppointmentId,
               
        });
        }


        public void UpdateX(SesstionDTO sesstion)
        {
            SesstionRepository.Update(new Session
            {
                Id = sesstion.Id,
                SessionId = sesstion.SessionId,
                UserId = sesstion.UserId,
                StartedDate = sesstion.StartedDate,
                Duration = sesstion.Duration,
                AppointmentId = sesstion.AppointmentId,
                IsPaymentConfirmed = sesstion.IsPaymentConfirmed,
                amount_cents = sesstion.amount_cents,
                created_at = sesstion.created_at,
                order = sesstion.order,
                profile_id = sesstion.profile_id,
                hmac = sesstion.hmac,
                IsStarted = sesstion.IsStarted,
                IsFinished = sesstion.IsFinished,
                merchant_order_id = sesstion.merchant_order_id
            }) ;
        }

        public SesstionDTO GetSesstionById(string id)
        {
            if(string.IsNullOrEmpty(id))
                return null;

            var sesstion = SesstionRepository.Get(s => s.SessionId == id,includeProperties: "Appointment").FirstOrDefault();
            var duration = new TimeSpan(0, 0, sesstion.Duration, 0);
            sesstion.Appointment.Date = sesstion.Appointment.Date.Add(sesstion.Appointment.Time);


            //if (sesstion.Appointment.Date > DateTime.Now.ToEgyptTimezone())
            //{
            //    return new SesstionDTO { ErrorCode = 2, ErrorMessage = "Session not started yet." };
            //}

			//gendy test 1
            //if (sesstion.Appointment.Date.Add(duration) < DateTime.Now.ToEgyptTimezone())
            //{
            //    return new SesstionDTO {IsFinished=true, ErrorCode = 1, ErrorMessage = "Session is finished." };
            //}


            return new SesstionDTO
            {
                Id = sesstion.Id,
                SessionId=sesstion.SessionId,
                UserId = sesstion.UserId,
                StartedDate = sesstion.StartedDate,
                Duration=sesstion.Duration,
                DurationMS = sesstion.Appointment.Date.Add(duration).Subtract(DateTime.Now.ToEgyptTimezone()).TotalMilliseconds,
                AppointmentId = sesstion.AppointmentId,
				//IsStarted = DateTime.Now.ToEgyptTimezone() >= sesstion.Appointment.Date  && DateTime.Now.ToEgyptTimezone() < sesstion.Appointment.Date.Add(duration) ,
				IsStarted = true,
				IsFinished = sesstion.IsFinished,
                IsPaymentConfirmed = sesstion.IsPaymentConfirmed,
                Appointment = new AppointmentDTO
                {
                    Id = sesstion.Appointment.Id,
                    Date = sesstion.Appointment.Date,
                    Time = sesstion.Appointment.Time,
                    LawyerId=sesstion.Appointment.LawyerId
                }
            };
        }

		public IEnumerable<SesstionDTO> GetPendingLiveSesstions(string UserId)
		{
			var res = SesstionRepository.Get(s => s.UserId == UserId &&
			!s.IsStarted, includeProperties: "Appointment,Appointment.Lawyer.User").OrderByDescending(s => s.Appointment.Date.Add(s.Appointment.Time));
			var result = res.Select(s => new SesstionDTO
			{
				Id = s.Id,
				SessionId = s.SessionId,
				UserId = s.UserId,
				StartedDate = s.StartedDate,
				Duration = s.Duration,
				AppointmentId = s.AppointmentId,
				Appointment = new AppointmentDTO
				{
					Id = s.Appointment.Id,
					Date = s.Appointment.Date,
					DateTxt = s.Appointment.Date.ToString("dddd, dd MMMM yyyy"),
					LawyerId = s.Appointment.LawyerId,
					Time = s.Appointment.Time,
					TimeFrom = new DateTime().Add(s.Appointment.Time).ToString("hh:mm tt"),
					TimeTo = new DateTime().Add(s.Appointment.Time.Add(new TimeSpan(0, s.Duration, 0))).ToString("hh:mm tt"),
					LawyerName = s.Appointment.Lawyer.User.Name,
					LawyerNameEn = s.Appointment.Lawyer.User.NameEn,
					Amount = s.Appointment.Lawyer.Fees.HasValue ? s.Appointment.Lawyer.Fees.Value : 0,
					Discount = 0
				}
			});



			return result;
		}


		public IEnumerable<SesstionDTO> GetLawyerPendingLiveSesstions(int LawyerId)
		{
			var res = SesstionRepository.Get(s => s.Appointment.LawyerId == LawyerId &&
			!s.IsStarted, includeProperties: "Appointment,Appointment.Lawyer.User").OrderByDescending(s => s.Appointment.Date.Add(s.Appointment.Time));
			var result = res.Select(s => new SesstionDTO
			{
				Id = s.Id,
				SessionId = s.SessionId,
				UserId = s.UserId,
				StartedDate = s.StartedDate,
				Duration = s.Duration,
				AppointmentId = s.AppointmentId,
				Appointment = new AppointmentDTO
				{
					Id = s.Appointment.Id,
					Date = s.Appointment.Date,
					DateTxt = s.Appointment.Date.ToString("dddd, dd MMMM yyyy"),
					LawyerId = s.Appointment.LawyerId,
					Time = s.Appointment.Time,
					TimeFrom = new DateTime().Add(s.Appointment.Time).ToString("hh:mm tt"),
					TimeTo = new DateTime().Add(s.Appointment.Time.Add(new TimeSpan(0, s.Duration, 0))).ToString("hh:mm tt"),
					LawyerName = s.Appointment.Lawyer.User.Name,
					LawyerNameEn = s.Appointment.Lawyer.User.NameEn,
					Amount = s.Appointment.Lawyer.Fees.HasValue ? s.Appointment.Lawyer.Fees.Value : 0,
					Discount = 0
				},
				UserNameAr = s.User.Name,
				UserName = s.User.NameEn
			});



			return result;
		}
		public SesstionDTO GetSesstionByAppointmentId(int id)
        {
            var session= SesstionRepository.Get(s => s.AppointmentId == id,includeProperties: "Appointment").FirstOrDefault();
            if (session == null)
                return null;

            return new SesstionDTO
            {
                Id = session.Id,
                SessionId = session.SessionId,
                UserId = session.UserId,
                StartedDate = session.StartedDate,
                Duration = session.Duration,
                AppointmentId = session.AppointmentId,
                OrderId= session.OrderId,
                merchant_order_id = session.merchant_order_id,
                Appointment = new AppointmentDTO
                {
                    Id = session.Appointment.Id,
                    LawyerId = session.Appointment.LawyerId,
                    Time = session.Appointment.Time,
                    TimeFrom = new DateTime().Add(session.Appointment.Time).ToString("hh:mm tt")
                }
            };
        }


        public SesstionDTO GetSesstionBymerchantId(int id)
        {
            var session = SesstionRepository.Get(s => s.merchant_order_id == id.ToString(), includeProperties: "Appointment").FirstOrDefault();
            if (session == null)
                return null;

            return new SesstionDTO
            {
                Id = session.Id,
                SessionId = session.SessionId,
                UserId = session.UserId,
                StartedDate = session.StartedDate,
                Duration = session.Duration,
                AppointmentId = session.AppointmentId,
                OrderId = session.OrderId,
                merchant_order_id = session.merchant_order_id,
                Appointment = new AppointmentDTO
                {
                    Id = session.Appointment.Id,
                    LawyerId = session.Appointment.LawyerId,
                    Time = session.Appointment.Time,
                    TimeFrom = new DateTime().Add(session.Appointment.Time).ToString("hh:mm tt")
                }
            };
        }
        public SesstionDTO GetSesstionByOrderId(string id)
		{
			var session = SesstionRepository.Get(s => s.merchant_order_id == id, includeProperties: "Appointment").FirstOrDefault();
			if (session == null)
				return null;

			return new SesstionDTO
			{
				Id = session.Id,
				SessionId = session.SessionId,
				UserId = session.UserId,
				StartedDate = session.StartedDate,
				Duration = session.Duration,
				AppointmentId = session.AppointmentId,
				Appointment = new AppointmentDTO
				{
					Id = session.Appointment.Id,
					LawyerId = session.Appointment.LawyerId,
					Time = session.Appointment.Time,
					TimeFrom = new DateTime().Add(session.Appointment.Time).ToString("hh:mm tt")
				},
				merchant_order_id = session.merchant_order_id,
				OrderId = session.OrderId,
				IsPaymentConfirmed = session.IsPaymentConfirmed
			};
		}


		public void RateSession(string sessionId,string userId,int rate,string review)
        {
            var session = SesstionRepository.Get(s => s.SessionId == sessionId, includeProperties: "Appointment").FirstOrDefault();
            var lawyer = LawyerRepository.Get(s=>s.Id==session.Appointment.LawyerId,includeProperties:"Reviews").FirstOrDefault();
            lawyer.Reviews.Add(new Review { Rate = rate, Title = review, UserId = userId,LawyerId= lawyer.Id });
            LawyerRepository.Update(lawyer);
        }

        public SesstionDTO EndSession(string id)
        {
            var sesstion = SesstionRepository.Get(s => s.SessionId == id).FirstOrDefault();
            sesstion.IsFinished = true;
            SesstionRepository.Update(sesstion);

            return new SesstionDTO
            {
                Id = sesstion.Id,
                SessionId=sesstion.SessionId,
                UserId = sesstion.UserId,
                StartedDate = sesstion.StartedDate,
                Duration = sesstion.Duration,
                AppointmentId = sesstion.AppointmentId,
                IsFinished=sesstion.IsFinished
            };

        }

        public void InsertNote(SessionNoteDTO note)
        {
            SessionNoteRepository.Insert(SessionNoteDTO.ToSessionNote(note));
        }

    }
}
