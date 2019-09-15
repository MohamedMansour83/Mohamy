using ElMaitre.DAL.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ElMaitre.DTO
{
    public class ServiceDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("TitleEn")]
        public string TitleEn { get; set; }

        [JsonProperty("PaperWork")]
        public string PaperWork { get; set; }

        [JsonProperty("ContactNumber")]
        public string ContactNumber { get; set; }

        [JsonProperty("Duration")]
        public int Duration { get; set; }

        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }



        [JsonProperty("Price")]
        public double Price { get; set; }

        [JsonProperty("PriceLawyer")]
        public string PriceLawyer { get; set; }

        [JsonProperty("PriceLevel2Lawyer")]
        public string PriceLevel2Lawyer { get; set; }

        public bool CanReserved { get; set; }
        public int IdentId { get; set; }


        public static DAL.Models.Service ToService(ServiceDTO serv)
        {

            return new DAL.Models.Service
            {
                Title = serv.Title,
                TitleEn = serv.TitleEn,
                Duration = serv.Duration,
                PaperWork = serv.PaperWork,
                CategoryId = serv.CategoryId,
                ContactNumber=serv.ContactNumber
            };
        }

        public static ServiceDTO ToServiceDTO(DAL.Models.Service serv, Lawyer lawyer = null)
        {
            var price = "0";
            var price2 = "0";
            var transId = 0;
            if (lawyer != null)
            {
                if (lawyer.Services != null)
                {
                    var chk = lawyer.Services.FirstOrDefault(sl => sl.ServiceId == serv.Id);
                    if (chk != null)
                    {
                        transId = chk.Id;
                        price = chk.PriceProvided.ToString();
                        price2 = chk.PriceLevel2Provided.ToString();
                    }
                }
            }
            var s = new ServiceDTO
            {
                Id = serv.Id,
                Title = serv.Title,
                TitleEn = serv.TitleEn,
                Duration = serv.Duration,
                PaperWork = serv.PaperWork,
                CategoryId = serv.CategoryId,
                ContactNumber = serv.ContactNumber,
                PriceLawyer = price,
                PriceLevel2Lawyer = price2,
                IdentId = transId
            };

            if (serv.Prices != null && lawyer != null)
            {
                var p = serv.Prices.Where(w => w.ExperienceId == lawyer.ExperienceId).FirstOrDefault();
                if (p != null)
                    s.Price = p.Price;
            }
            return s;
        }

    }
}
