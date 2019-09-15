using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IPriceRangeService
    {
        IEnumerable<PriceRangeDTO> Get();
    }
}
