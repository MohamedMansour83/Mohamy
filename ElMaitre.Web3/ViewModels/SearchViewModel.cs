using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class SearchViewModel
    {
        public int ServiceId { get; set; }
        public int Specialization { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public int? Isonline { get; set; }
        public List<int> Rating { get; set; }
        public List<int> Prices { get; set; }
        public List<int> Experiences { get; set; }
        public string MinFees { get; set; }
        public string MaxFees { get; set; }

    }
}
