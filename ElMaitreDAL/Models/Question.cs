using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Question")]
    public class Question : BaseModel
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string AnonymousEmail { get; set; }
        public string AnonymousName { get; set; }

        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<QuestionAnswer> Answers { get; set; }


    }
}
