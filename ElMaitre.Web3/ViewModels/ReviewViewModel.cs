using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class ReviewViewModel
    {
        public string SessionId { get; set; }
        public string Review { get; set; }
        public int Rate { get; set; }
    }
}
