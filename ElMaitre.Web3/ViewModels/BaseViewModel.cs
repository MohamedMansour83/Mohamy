using ElMaitre.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class BaseViewModel
    {

        public bool IsLtr
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en";
            }
        }

        public bool IsAuthenticated { get; set; }
        public bool IsLawyer { get; set; }

        public string UserName { get; set; }
        public Lawyer Lawyer { get; set; }
		public Session Session { get; set; }
		//public string FbId { get; set; }

		public BaseViewModel( ) { }
        public BaseViewModel(HttpRequest request)
        {
            IsAuthenticated = request.Cookies.Any(s => s.Key == "token");
            if (IsAuthenticated)
            {
                IsLawyer = Convert.ToBoolean(request.Cookies["IsLawyer"]);
                UserName = IsLtr? request.Cookies["UserNameEn"]: request.Cookies["UserName"];
            }
        }
    }

}
