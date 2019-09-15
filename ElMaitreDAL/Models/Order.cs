using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Models
{
    [Table("Order")]
    public class Order : BaseModel
    {
        public Order()
        {
            AddedDate = DateTime.Now;
        }

        [Key]
        public new int Id { get; set; }
        public new DateTime AddedDate { get; set; }
        public new DateTime ModifiedDate { get; set; }

        public int serviceId { get; set; }
        public virtual Service service { get; set; }
        public int lawyerId { get; set; }
        public virtual Lawyer lawyer { get; set; }
        public string clientId { get; set; }
        public virtual ApplicationUser client { get; set; }

        public string created_at { get; set; }
        public string currency { get; set; }
        public bool? is_3d_secure { get; set; }
        public bool? is_void { get; set; }
        public bool? pending { get; set; }
        public bool? is_capture { get; set; }
        public bool? is_refund { get; set; }
        public bool? error_occured { get; set; }
        public int? txn_response_code { get; set; }
        public int? profile_id { get; set; }
        public string message { get; set; }
        public bool? is_auth { get; set; }
        public int? refunded_amount_cents { get; set; }
        public string hmac { get; set; }
        public int? captured_amount { get; set; }
        public bool? success { get; set; }
        public bool? has_parent_transaction { get; set; }
        public string pan { get; set; }
        public int? order { get; set; }
        public bool? is_voided { get; set; }
        public int? ordtrans_id { get; set; }
        public string type { get; set; }
        public int? integration_id { get; set; }
        public bool? is_standalone_payment { get; set; }
        public bool? is_refunded { get; set; }
        public int? acq_response_code { get; set; }
        public string sub_type { get; set; }
        public int? amount_cents { get; set; }
        public int? owner { get; set; }
        public int? merchant_order_id { get; set; }
    }
}
