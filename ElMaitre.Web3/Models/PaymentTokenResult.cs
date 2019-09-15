using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public  class PaymentTokenResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }

    public partial class Profile
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
