using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElMaitre.Extensions;
using ElMaitre.DAL.Data;

namespace ElMaitre.Services
{
    public class LawyerService : ILawyerService
    {
        private IRepository<Lawyer> LawyerRepository;
        private IRepository<Review> ReviewRepository;
        private ILawyerAppointmentService lawyerAppointmentService;
        private IRepository<LawyerSpecialization> LawyerSpecializationRepository;
        private IRepository<PriceRange> PriceRangeRepository;
        private IRepository<LawyerExperience> LawyerExperienceRepository;

        public LawyerService(IRepository<Lawyer> LawyerRepository, IRepository<LawyerSpecialization> LawyerSpecializationRepository,
            ILawyerAppointmentService lawyerAppointmentService, IRepository<PriceRange> PriceRangeRepository, IRepository<Review> ReviewRepository,
            IRepository<LawyerExperience> LawyerExperienceRepository)
        {
            this.LawyerRepository = LawyerRepository;
            this.LawyerSpecializationRepository = LawyerSpecializationRepository;
            this.lawyerAppointmentService = lawyerAppointmentService;
            this.PriceRangeRepository = PriceRangeRepository;
            this.ReviewRepository = ReviewRepository;
            this.LawyerExperienceRepository = LawyerExperienceRepository;
        }

        public IEnumerable<LawyerDTO> GetLawyers()
        {
            return LawyerRepository.Get(s=>s.SpecializationId!=null&& 
            s.Fees!=null && s.Rate!=null).
            Select(s => LawyerDTO.ToLawyerDTO(s));
        }

        public LawyerDTO GetLawyer(long id)
        {
            if (id != 0)
            {
                var lawyer = LawyerRepository.Get(s => s.Id == id, includeProperties: "Experience,Specialization,User,Reviews,Reviews.User").Select(l => LawyerDTO.ToLawyerDTO(l)).FirstOrDefault();
                lawyer.Appointments = lawyerAppointmentService.GetGroupedAppointments(lawyer.Id);
                if (lawyer.Appointments == null || lawyer.Appointments.Count() == 0)
                {
                    lawyer.Appointments = new List<AppointmentGroupedDTO> { new AppointmentGroupedDTO { DateTxt = "" } };
                }

                return lawyer;
            }
            else
            {
                return new LawyerDTO();
            }
        }


        public RatingDTO GetRating(long id)
        {
            var lawyer = LawyerRepository.Get(s => s.Id == id, includeProperties: "Reviews,Reviews.User,Reviews.Replies,Reviews.Replies.User").FirstOrDefault();
            var reviews = lawyer.Reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Rate = r.Rate,
                Title = r.Title,
                UserId = r.UserId,
                UserName = r.User.UserName,
                Name=r.User.Name,
                NameEn=r.User.NameEn,
                Replies = r.Replies?.Select(w => ReplyDTO.ToReviewReplyDTO(w))
            });
            return new RatingDTO
            {
                LawyerRate = LawyerDTO.CalculateRate(lawyer.Reviews).Value,
                Rate1 = lawyer.Reviews.Where(s => s.Rate == 1).Count(),
                Rate2 = lawyer.Reviews.Where(s => s.Rate == 2).Count(),
                Rate3 = lawyer.Reviews.Where(s => s.Rate == 3).Count(),
                Rate4 = lawyer.Reviews.Where(s => s.Rate == 4).Count(),
                Rate5 = lawyer.Reviews.Where(s => s.Rate == 5).Count(),
                TotalRating = lawyer.Reviews.Count(),
                Reviews = reviews
            };
        }

        public int InsertLawyer(LawyerDTO lawyer)
        {
            LawyerRepository.Insert(LawyerDTO.ToLawyer(lawyer));

            return LawyerRepository.Get(s => s.UserId == lawyer.UserId).First().Id;

        }
        public void UpdateLawyer(LawyerDTO lawyer)
        {
            var law = LawyerRepository.Get(lawyer.Id);
            law.Certificates = lawyer.Certificates;
            law.Description = lawyer.Description;
            law.CertificatesEn = lawyer.CertificatesEn;
            law.DescriptionEn = lawyer.DescriptionEn;
            law.Fees = lawyer.Fees;
            law.IsOnline = lawyer.IsOnline;
            law.Rate = lawyer.Rate;
            law.SpecializationId = lawyer.SpecializationId;
            law.UserId = lawyer.UserId;
            law.ModifiedDate = lawyer.ModifiedDate.HasValue ? lawyer.ModifiedDate.Value : DateTime.Now;
            law.ExperienceId = lawyer.ExperienceId;
            law.VideoURL = lawyer.VideoURL;
            LawyerRepository.Update(law);
        }

        //public void DeleteLawyer(long id)
        //{
        //    Lawyer Lawyer = GetLawyer(id);
        //    LawyerRepository.Delete(Lawyer);
        //}


        public IEnumerable<LawyerDTO> GetLawyers(int ServiceId,string Name, int Specialization,Gender? Gender, List<int> Rating, float minFees, float maxFees, List<int> Prices, bool? isOnline, List<int> Experiences)
        {
            Expression<Func<Lawyer, bool>> filter = null;

            filter = s => s.SpecializationId != null && s.Fees != null && s.Rate != null;


            if (!string.IsNullOrEmpty(Name) && Specialization > 0)
            {
                Expression<Func<Lawyer, bool>> filterToAppend = s => s.User.Name.ToLower().Contains(Name.ToLower()) && s.SpecializationId == Specialization;
                filter = filter.And(filterToAppend);
            }
            else
            {
                if (!string.IsNullOrEmpty(Name))
                    filter= filter.And( s => s.User.Name.ToLower().Contains(Name.ToLower()));

                if (Specialization > 0)
                    filter = filter.And(s => s.SpecializationId == Specialization);
            }

            if (ServiceId >0)
                filter = filter.And(s => s.Services.Any(a=>a.ServiceId==ServiceId));

            if (Gender != null)
                filter = filter.And(s => s.User.Gender == Gender);

            if (isOnline.HasValue)
                if (isOnline.Value)
                    filter = filter.And(s => s.ModifiedDate.AddMinutes(20) >= DateTime.Now);
                else
                    filter = filter.And(s => s.ModifiedDate.AddMinutes(20) < DateTime.Now);

            if (Rating != null && Rating.Count>0)
                filter = filter.And(s => Rating.Contains(LawyerDTO.CalculateRate(s.Reviews).Value));

            if (minFees>0)
                filter = filter.And(s => s.Fees>=minFees);

            if (maxFees > 0)
                filter = filter.And(s => s.Fees <= maxFees);

            if (Prices!=null && Prices.Count > 0)
            {
                var lst = new List<PriceRange>();
                foreach (var item in Prices)
                {
                    lst.Add(PriceRangeRepository.Get(item));
                }
                lst = lst.OrderBy(s => s.From).ToList();
                filter = filter.And(s => s.Fees >= lst.First().From && s.Fees <= lst.Last().To);
            }

            if (Experiences != null && Experiences.Count > 0)
            {
                //Expression<Func<Lawyer, bool>> Expfilter = null;

                //foreach (var item in Experiences)
                //{
                //    if (Expfilter == null)
                //        Expfilter = s => s.ExperienceId == item;
                //    else
                //        Expfilter=Expfilter.a
                //}

                filter = filter.And(s=> Experiences.Contains(s.ExperienceId.Value));

            }


            var res = LawyerRepository.Get(filter, includeProperties: "Experience,Specialization,User,Reviews,Reviews.User,Services").Select(s => LawyerDTO.ToLawyerDTO(s)).ToList();

            foreach (var item in res)
            {
                item.Appointments  = lawyerAppointmentService.GetGroupedAppointments(item.Id);
                if (item.Appointments == null || item.Appointments.Count()==0)
                {
                    item.Appointments = new List<AppointmentGroupedDTO> { new AppointmentGroupedDTO {  DateTxt=""} };
                }
            }

            return res;
        }



        public IEnumerable<LawyerSpecializationDTO> GetSpetializations()
        {
            var lst = new List<int> { 22, 20, 15, 19, 17, 6, 14, 5, 3, 4 };
            return LawyerSpecializationRepository
                .Get(s => lst.Contains(s.Id)).
                Select(s => new LawyerSpecializationDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    NameEn = s.NameEn
                });
        }

        public LawyerDTO GetLawyerByUserId(string id)
        {
            var lawyer = LawyerRepository.Get(s => s.UserId == id).FirstOrDefault();
            return GetLawyer(lawyer.Id);
        }

        public LawyerDTO GetLawyerByFbId(string id)
        {
            var lawyer = LawyerRepository.Get(s => s.User.FbId == id).FirstOrDefault();
            if (lawyer == null)
                return null;
            return GetLawyer(lawyer.Id);
        }

        public IEnumerable<KeyValueDTO> GetLawyerExperiences()
        {
            return LawyerExperienceRepository.Get().Select(s=>new KeyValueDTO { Id = s.Id, Value = s.Name, ValueEn = s.NameEn, Price = s.Price });
        }
    }
}
