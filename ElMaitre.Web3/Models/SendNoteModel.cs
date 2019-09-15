using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public class SendNoteModel
    {
        public int SessionId { get; set; }
        public string Note { get; set; }
    }
}
