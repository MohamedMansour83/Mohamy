using ElMaitre.DAL.Models;
using ElMaitre.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElMaitre.DTO
{
    public class LawyerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool? IsOnline { get; set; }
        public int? Rate { get; set; }
        public double? Fees { get; set; }
        public string Description { get; set; }
        public string Certificates { get; set; }
        public string Specialization { get; set; }
        public string SpecializationEn { get; set; }
        public string CertificatesEn { get; set; }
        public string DescriptionEn { get; set; }
        public int? SpecializationId { get; set; }
        public int? ExperienceId { get; set; }
        public string ExperienceName { get; set; }
        public string ExperienceNameEn { get; set; }
        public string UserId { get; set; }
        public string ProfileImg { get; set; }
        public string VideoURL { get; set; }
        public string Video { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<AppointmentGroupedDTO> Appointments { get; set; }
        public IEnumerable<ReviewDTO> Reviews { get; set; }
        public IEnumerable<ServiceDTO> Services { get; set; }

        public static Lawyer ToLawyer(LawyerDTO lawyer)
        {

            return new Lawyer
            {
                Id = lawyer.Id,
                Certificates = lawyer.Certificates,
                Description = lawyer.Description,
                CertificatesEn = lawyer.CertificatesEn,
                DescriptionEn = lawyer.DescriptionEn,
                Fees = lawyer.Fees,
                IsOnline = lawyer.IsOnline,
                Rate = lawyer.Rate,
                SpecializationId = lawyer.SpecializationId,
                UserId = lawyer.UserId,
                ModifiedDate = lawyer.ModifiedDate.HasValue ? lawyer.ModifiedDate.Value : DateTime.Now,
                ExperienceId = lawyer.ExperienceId,
            };
        }

        public static LawyerDTO ToLawyerDTO(Lawyer lawyer)
        {

            return new LawyerDTO
            {
                Id = lawyer.Id,
                UserId = lawyer.UserId,
                Certificates = lawyer.Certificates,
                Description = lawyer.Description,
                CertificatesEn = lawyer.CertificatesEn,
                DescriptionEn = lawyer.DescriptionEn,
                Fees = lawyer.Fees,
                IsOnline = lawyer.ModifiedDate.AddMinutes(20) >= DateTime.Now,
                Name = lawyer.User.Name,
                NameEn = lawyer.User.NameEn,
                Rate = CalculateRate(lawyer.Reviews),
                Specialization = lawyer.Specialization != null ? lawyer.Specialization.Name : "",
                SpecializationEn = lawyer.Specialization != null ? lawyer.Specialization.NameEn : "",
                SpecializationId = lawyer.SpecializationId,
                Reviews = lawyer.Reviews.Select(r => ReviewDTO.ToReviewDTO(r)),
                ExperienceId = lawyer.ExperienceId,
                ExperienceName = lawyer.Experience != null ? lawyer.Experience.Name : "",
                ExperienceNameEn = lawyer.Experience != null ? lawyer.Experience.NameEn : "",
                VideoURL = lawyer.VideoURL,
                Video = Utils.GetVideo(lawyer.VideoURL),
                ProfileImg = string.IsNullOrEmpty(lawyer.User.ProfileImg) ? "/images/personal-img.jpg" : lawyer.User.ProfileImg,
            };
        }


       public static int? CalculateRate(ICollection<Review> reviews)
        {
            if (reviews == null || reviews.Count == 0)
                return 0;

            var rates = reviews.GroupBy(s => s.Rate)
                .Select(
                        g => new
                        {
                            Key = g.Key,
                            Count = g.Count(),
                        }).ToList();

            float average = 0;

            for (int i = 0; i < rates.Count; i++)
            {
                average += rates[i].Count * rates[i].Key;
            }

            average /= rates.Sum(s => s.Count);


            return Convert.ToInt32(average);
        }

    }
}
