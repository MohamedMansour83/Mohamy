using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.Service.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public int ErrorCode{ get; set; }
        public string ErrorMessage { get; set; }
    }
}
