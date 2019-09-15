using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public string token { get; set; }
        public string res { get; set; }
        public UserProfileViewModel User { get; set; }
        public ProfileViewModel(HttpRequest request) : base(request)
        {

        }
    }

}
