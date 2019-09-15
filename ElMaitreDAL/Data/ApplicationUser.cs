using ElMaitre.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public string ProfileImg { get; set; }

        public int? LawyerId { get; set; }
        public virtual Lawyer Lawyer { get; set; }

        public int? ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Document> SentDocuments { get; set; }
        public virtual ICollection<Document> ReceivedDocuments { get; set; }
        public virtual ICollection<SessionNote> SessionNotes { get; set; }

        public string FbId { get; set; }
    }
}

public enum Gender { Male = 0, Female = 1 }
