using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface ICountryService
    {
        IEnumerable<KeyValueDTO> GetProvinces();
    }
}
