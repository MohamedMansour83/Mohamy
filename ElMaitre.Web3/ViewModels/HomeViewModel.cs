using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public string Token { get; set; }
        public HomeViewModel(HttpRequest request) : base(request)
        {

        }
    }

}
