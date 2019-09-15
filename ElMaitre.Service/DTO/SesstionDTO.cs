using ElMaitre.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DTO
{
    public class SesstionDTO:BaseDTO
    {
        public string SessionId { get; set; }
        public int Duration { get; set; }
        public double DurationMS { get; set; }
        public DateTime? StartedDate { get; set; }
        public int AppointmentId { get; set; }
        public bool IsStarted { get; set; }
        public bool IsPaymentConfirmed { get; set; }
        public bool IsFinished { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
		public string merchant_order_id { get; set; }
		public int amount_cents { get; set; }
		public string UserName { get; set; }
		public string UserNameAr { get; set; }
		public string hmac { get; set; }
		public int profile_id { get; set; }
		public string created_at { get; set; }
		public int order { get; set; }
		public AppointmentDTO Appointment { get; set; }

        public IEnumerable<SessionNoteDTO> Notes { get; set; }

        public bool IsGeust { get; set; }

    }
}
