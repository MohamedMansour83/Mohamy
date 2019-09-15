using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class ReviewReplyViewModel
    {
        public int ReviewId { get; set; }
        public string Reply { get; set; }
        public string UserId { get; set; }
    }
}
