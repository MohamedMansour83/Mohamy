using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DTO
{
    public class ServiceResponse
    {
        public bool ResponseStatus { get; set; }
        public ServiceResponseCode Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ArabicMessage { get; set; }
    }
}
