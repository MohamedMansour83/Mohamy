using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public partial class OrderResult
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("delivery_needed")]
        public bool DeliveryNeeded { get; set; }

        [JsonProperty("collector")]
        public object Collector { get; set; }

        [JsonProperty("amount_cents")]
        public long AmountCents { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("is_payment_locked")]
        public bool IsPaymentLocked { get; set; }

        [JsonProperty("merchant_order_id")]
        public object MerchantOrderId { get; set; }

        [JsonProperty("wallet_notification")]
        public object WalletNotification { get; set; }

        [JsonProperty("paid_amount_cents")]
        public long PaidAmountCents { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }
    }
}
