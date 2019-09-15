using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class SendQuestionViewModel
    {
        public string Question { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public string AnonymousEmail { get; set; }
        public string AnonymousName { get; set; }


    }
}
