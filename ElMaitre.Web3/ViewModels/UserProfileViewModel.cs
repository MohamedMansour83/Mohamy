using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public int? LawyerId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string VideoURL { get; set; }
        public int Age { get; set; }
        public string DateOfBirth { get; set; }
        public IEnumerable<SesstionDTO> Sessions { get; set; }
        public IEnumerable<DocumentDTO> Documents { get; set; }
        public Gender Gender { get;  set; }
        public IEnumerable<LawyerSpecializationDTO> Spetializations { get; set; }
        public int? SpetializationId { get; set; }
        public string Description { get; set; }
        public string Certificates { get; set; }

        public double? Price { get; set; }

        public RatingDTO Rating { get; set; }
        public string DescriptionEn { get;  set; }
        public string CertificatesEn { get;  set; }
        public int? ProvinceId { get;  set; }
        public int? ExperienceId { get;  set; }
        public string PhoneNumber { get;  set; }
        public string ProfileImg { get;  set; }



        public IEnumerable<KeyValueDTO> Provinces { get; set; }
        public IEnumerable<KeyValueDTO> Experiences { get; set; }
        public IEnumerable<ServiceDTO> Services { get; set; }
        public IEnumerable<ServiceCategoryDTO> ServiceCategories { get; set; }


    }
}
