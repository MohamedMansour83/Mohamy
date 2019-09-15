using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Models
{
    public class CashPayResult
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("amount_cents")]
        public long AmountCents { get; set; }

        [JsonProperty("merchant_order_id")]
        public object MerchantOrderId { get; set; }

        [JsonProperty("redirection_url")]
        public string redirection_url { get; set; }

        [JsonProperty("hmac")]
        public string hmac { get; set; }
    }
}
