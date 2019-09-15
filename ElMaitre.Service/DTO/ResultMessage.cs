using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.Service.DTO
{
    public class ResultMessage
    {
        public bool IsSuccess { get { return string.IsNullOrEmpty(ErrorMessage); } }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
    }
}
