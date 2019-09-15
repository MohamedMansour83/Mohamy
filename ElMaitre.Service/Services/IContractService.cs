using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IContractService
    {
        IEnumerable<ContractDTO> Get(int? categoryId,string name);
        ContractDTO Get(int Id);
        IEnumerable<KeyValueDTO> GetCategories();
    }
}
