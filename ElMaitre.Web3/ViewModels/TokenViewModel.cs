using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class TokenViewModel
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public bool IsLawyer { get; set; }
        public string UserName { get; set; }
        public string UserNameEn { get; set; }
        public string Message { get; set; }

    }
}
