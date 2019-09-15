using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ElMaitre.DTO;
using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.Service.DTO;
using ElMaitre.Service.Extensions;

namespace ElMaitre.Services
{
    public class LawyerAppointmentService : ILawyerAppointmentService
    {
        private IRepository<LawyerAppointment> LawyerAppointmentRepository;
        private IRepository<Session> SessionRepository;

        const int sessionDuration = 30;

        public LawyerAppointmentService(IRepository<LawyerAppointment> LawyerAppointmentRepository, IRepository<Session> SessionRepository)
        {
            this.LawyerAppointmentRepository = LawyerAppointmentRepository;
            this.SessionRepository = SessionRepository;
        }


        public AppointmentDTO GetAppointmentDetails(int Id)
        {
            return LawyerAppointmentRepository.Get(w => w.Id == Id, includeProperties: "Lawyer,Lawyer.User").Select(s => new AppointmentDTO
            {
                Id = s.Id,
                Date = s.Date,
                DateTxt = s.Date.ToString("dddd, dd MMMM yyyy"),
                LawyerId = s.LawyerId,
                Time = s.Time,
                TimeFrom = new DateTime().Add(s.Time).ToString("hh:mm tt"),
                TimeTo = new DateTime().Add(s.Time).Add(new TimeSpan(0, sessionDuration, 0)).ToString("hh:mm tt"),
                LawyerName = s.Lawyer.User.Name,
                LawyerNameEn = s.Lawyer.User.NameEn,
                Amount = s.Lawyer.Fees.HasValue ? s.Lawyer.Fees.Value : 0,
                Discount = 0,
                CanReserved = !s.Sessions.Any()
            }).FirstOrDefault();
        }

        public IEnumerable<AppointmentDTO> GetAppointments(string userId,int lawyerId)
        {
            //var sessions = SessionRepository.Get(w => w.UserId == userId);
            //&& !sessions.Any(s=>s.AppointmentId==w.Id)
            return LawyerAppointmentRepository.Get(w => w.LawyerId == lawyerId && w.Date.Add(w.Time).Add(new TimeSpan(0,0,sessionDuration, 0))>DateTime.Now.ToEgyptTimezone()).Select(s => new AppointmentDTO
            {
                Id = s.Id,
                Date = s.Date,
                LawyerId = s.LawyerId,
                Time = s.Time,
                TimeFrom = new DateTime().Add(s.Time).ToString("hh:mm tt")
            });
            
        }

        public IEnumerable<AppointmentGroupedDTO> GetGroupedAppointments(int lawyerId)
        {
            var appointments = LawyerAppointmentRepository.Get(w => w.LawyerId == lawyerId && w.Date.Add(w.Time).Add(new TimeSpan(0, 0, sessionDuration, 0)) > DateTime.Now.ToEgyptTimezone() && !w.Sessions.Any()).OrderBy(d=>d.Date);
            var appointmentsGrouped= appointments.GroupBy(s => s.Date.Date).Select(s => new AppointmentGroupedDTO
            {
                Date = s.Key,
                DateTxt =s.Key.Date==DateTime.Now.ToEgyptTimezone().Date? "Today": DateTime.Now.ToEgyptTimezone().Date.Subtract(s.Key.Date).Days==-1 ?"Tomorrow" : s.Key.ToString("dddd, dd MMMM yyyy"),
                Times = s.Select(w => new TimeModel
                {
                    Id = w.Id,
                    Time = w.Time,
                    TimeFrom = new DateTime().Add(w.Time).ToString("hh:mm tt"),
                    TimeTo = new DateTime().Add(w.Time).Add(new TimeSpan(0,sessionDuration,0)).ToString("hh:mm tt")
                })
            });

            return appointmentsGrouped;
        }

        public ResultMessage SetAppointments(IEnumerable<AppointmentDTO> model)
        {
            string Msg = string.Empty;

            try
            {
                bool isvalid = ValidateTimes(model);
                if (!isvalid)
                    return new ResultMessage { ErrorMessage = "Invalid Date" };

                if (model.Any(s => s.Date.Date < DateTime.Now.ToEgyptTimezone().Date))
                    return new ResultMessage { ErrorMessage = "Invalid Date" };

                var ss = LawyerAppointmentRepository.Get(s => s.LawyerId == model.First().LawyerId && s.Date.Date == model.First().Date
                  && s.Date.Add(s.Time).Add(new TimeSpan(0, 0, sessionDuration, 0)) > DateTime.Now.ToEgyptTimezone(), includeProperties: "Sessions");
                List<AppointmentDTO> lstToAdd = new List<AppointmentDTO>();
                foreach (var item in model)
                {
                    if (!ss.Any(a => a.Time == item.Time))
                    {
                        if (item.Date.Add(item.Time) >= DateTime.Now.ToEgyptTimezone())
                        {
                            lstToAdd.Add(item);
                            Msg += $"<div class='row col-md-12' style='color:green'>- {new DateTime().Add(item.Time).ToString("hh:mm tt")}   تمت الاضاف بنجاح .</h7>";
                        }
                        else
                        {
                            Msg += $"<div class='row col-md-12' style='color:red'>- Cannot add {new DateTime().Add(item.Time).ToString("hh:mm tt")}.</h7>";
                        }
                    }
                }

                List<LawyerAppointment> lstToDelete = new List<LawyerAppointment>();
                foreach (var item in ss)
                {
                    if (!model.Any(a => a.Time == item.Time))
                    {
                        if (!item.Sessions.Any())
                        {
                            lstToDelete.Add(item);
                            Msg += $"<div class='row col-md-12' style='color:green'>- {new DateTime().Add(item.Time).ToString("hh:mm tt")} تم الحذف بنجاح .</h7>";
                        }
                        else
                        {
                            Msg += $"<div class='row col-md-12' style='color:red'>- Cannot delete {new DateTime().Add(item.Time).ToString("hh:mm tt")} already has sessions.</h7>";
                        }
                    }
                }

                LawyerAppointmentRepository.DeleteAll(lstToDelete);

                LawyerAppointmentRepository.InsertAll(lstToAdd.Select(s => new LawyerAppointment
                {
                    LawyerId = s.LawyerId,
                    Date = s.Date,
                    Time = s.Time,
                }));

            }catch(Exception ex)
            {
                //return new ResultMessage { ErrorMessage = ex.Message + "||" + ex.StackTrace };
                return new ResultMessage { ErrorMessage = "Somthing went wrong!!" };
            }

            return new ResultMessage { Message = Msg };

        }

        private bool ValidateTimes(IEnumerable<AppointmentDTO> model)
        {
            //var t = new List<string> { "8:00", "8:45", "9:30", "10:15","11:00","11:45","12:30","1:15","2:00","2:45","3:30","4:15","5:00","5:45","6:30","7:15" };
            //foreach (var item in model)
            //{
            //    if (!t.Contains(item.Time.ToString("hh:mm")))
            //    {
            //        return false;
            //    }
            //}

            return true;
        }
    }
}
