using System;
using System.Collections;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DateTxt { get; set; }
        public TimeSpan Time { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public int LawyerId { get; set; }
        public string LawyerName { get; set; }
        public string LawyerNameEn { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public bool CanReserved { get; set; }
    }

    public class AppointmentGroupedDTO
    {
        public DateTime Date { get; set; }
        public string DateTxt { get; set; }
        public IEnumerable<TimeModel> Times { get; set; }
    }

    public class TimeModel
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }
}
