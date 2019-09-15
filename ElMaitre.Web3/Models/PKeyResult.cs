using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public class PKeyResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
