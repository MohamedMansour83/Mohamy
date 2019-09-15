using ElMaitre.DAL.Models;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class PriceRangeDTO
    {
        public int Id { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public string Title { get { return From + " - " + To; } }

    }
}
