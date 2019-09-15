using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Lawyer")]
    public class Lawyer :BaseModel
    {
        public string Description { get; set; }
        public string DescriptionEn { get; set; }

        public bool? IsOnline { get; set; }

        public int? Rate { get; set; }

        public double? Fees { get; set; }

        public string Certificates { get; set; }
        public string CertificatesEn { get; set; }
        public string VideoURL { get; set; }

        public int? ExperienceId { get; set; }
        public virtual LawyerExperience Experience { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int? SpecializationId { get; set; }
        public virtual LawyerSpecialization Specialization { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<LawyerService> Services { get; set; }

    }



}
