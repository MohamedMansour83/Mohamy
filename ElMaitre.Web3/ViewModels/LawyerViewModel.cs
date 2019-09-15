using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class LawyerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Certificate { get; set; }
        public bool IsOnline { get; set; }
        public string Specialization { get; set; }
        public double Fees { get; set; }
        public int Rate { get; set; }
    }
}
