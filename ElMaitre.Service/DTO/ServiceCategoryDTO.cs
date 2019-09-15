using ElMaitre.DAL.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ElMaitre.DTO
{
    public class ServiceCategoryDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("NameEn")]
        public string NameEn { get; set; }

        [JsonProperty("IconPath")]
        public string IconPath { get; set; }

        

        [JsonProperty("Services")]
        public IEnumerable<ServiceDTO> Services { get; set; }

        public static ServiceCategory ToServiceCategory(ServiceCategoryDTO scat)
        {

            return new ServiceCategory
            {
                Name = scat.Name,
                NameEn = scat.NameEn,
                IconPath = scat.IconPath
            };
        }

        public static ServiceCategoryDTO ToServiceCategoryDTO(ServiceCategory scat, Lawyer lawyer = null)
        {

            return new ServiceCategoryDTO
            {
                Id=scat.Id,
                Name = scat.Name,
                NameEn = scat.NameEn,
                IconPath = scat.IconPath,
                Services = scat.Services.Select(s => ServiceDTO.ToServiceDTO(s, lawyer))
            };
        }

    }
}
