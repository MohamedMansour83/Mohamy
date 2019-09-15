using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public class AppSettings
    {
        public int PaymentIframeId { get; set; }
        public int PaymentIntegrationId { get; set; }
        public int PaymentIntegrationIdCash { get; set; }
		public int PaymentIntegrationId2 { get; set; }
		public int PaymentIntegrationIdCash2 { get; set; }
		public string PaymentApiKey { get; set; }
        public long MerchantId { get; set; }
    }
}
