using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DTO
{
   public class RatingDTO
    {
        public int LawyerRate { get; set; }
        public int Rate1 { get; set; }
        public int Rate2 { get; set; }
        public int Rate3 { get; set; }
        public int Rate4 { get; set; }
        public int Rate5 { get; set; }
        public int TotalRating { get; set; }
        public IEnumerable<ReviewDTO> Reviews { get; set; }
    }
}
