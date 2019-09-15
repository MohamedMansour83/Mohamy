using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("QuestionCategory")]
    public class QuestionCategory : BaseModel
    {
        public string Title { get; set; }
        public string TitleEn { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

    }
}
