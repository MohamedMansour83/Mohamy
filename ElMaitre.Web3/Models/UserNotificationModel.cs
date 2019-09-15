using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public class UserNotificationModel
    {
        public string SessionId { get; set; }
        public string LawyerName { get; set; }
		public string UserName { get; set; }
		public DateTime SessionTime { get; set; }
        public string lang { get; set; }
    }
}
