using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public class ReplyQuestionModel
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool UnAnswerQuestions { get; set; }
    }
}
