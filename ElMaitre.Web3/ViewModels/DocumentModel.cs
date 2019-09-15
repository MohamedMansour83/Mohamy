using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class DocumentModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Path")]
        public string Path { get; set; }
        [JsonProperty("Date")]
        public string Date { get; set; }
        
    }
}
