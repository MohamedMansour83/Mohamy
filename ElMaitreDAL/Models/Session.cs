using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Session")]
    public class Session : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SessionId { get; set; }
        public int Duration { get; set; }
        public DateTime? StartedDate { get; set; }

        public int AppointmentId { get; set; }
        public virtual LawyerAppointment Appointment { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPaymentConfirmed { get; set; }
        public string OrderId { get; set; }
        public virtual ICollection<SessionNote> Notes { get; set; }

		public int? ordtrans_id { get; set; }
		public int? profile_id { get; set; }
		public int? txn_response_code { get; set; }
		public string currency { get; set; }
		public bool? is_3d_secure { get; set; }
		public bool? is_void { get; set; }
		public bool? pending { get; set; }
		public bool? is_capture { get; set; }
		public bool? is_refund { get; set; }
		public string created_at { get; set; }
		public string hmac { get; set; }
		public int? captured_amount { get; set; }
		public bool? success { get; set; }
		public bool? has_parent_transaction { get; set; }
		public string pan { get; set; }
		public int? order { get; set; }
		public bool? is_voided { get; set; }
		public string type { get; set; }
		public int? integration_id { get; set; }
		public bool? is_standalone_payment { get; set; }
		public bool? is_refunded { get; set; }
		public int? acq_response_code { get; set; }
		public string sub_type { get; set; }
		public int? amount_cents { get; set; }
		public int? owner { get; set; }
		public string merchant_order_id { get; set; }
	}



}
