using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class AppointmentViewModel
    {
        public string Date { get; set; }
        public bool IsEmpty { get; set; }
        public List<AppointmentModel> Appointments { get; set; }
    }


    public class AppointmentModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
