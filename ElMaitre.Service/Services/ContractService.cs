using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using ElMaitre.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElMaitre.Services
{
    public class ContractService : IContractService
    {
        private IRepository<Contract> ContractRepository;
        private IRepository<ContractCategory> ContractCategoryRepository;

        public ContractService(IRepository<Contract> ContractRepository, IRepository<ContractCategory> ContractCategoryRepository)
        {
            this.ContractRepository = ContractRepository;
            this.ContractCategoryRepository = ContractCategoryRepository;
        }

        public IEnumerable<ContractDTO> Get(int? catId, string name)
        {
            Expression<Func<Contract, bool>> filter = s => s.Id > 0;

            if (catId != null && catId > 0)
            {
                filter = s => s.CategoryId == catId;
            }

            if (!string.IsNullOrEmpty(name))
            {
                filter = filter.And(s => s.Name.Contains(name));
            }

            return ContractRepository.Get(filter).Select(s=> ContractDTO.ToContractDTO(s));
        }

        public ContractDTO Get(int Id)
        {
            return ContractDTO.ToContractDTO( ContractRepository.Get(Id));
        }

        public IEnumerable<KeyValueDTO> GetCategories()
        {
            return ContractCategoryRepository.Get().Select(s => new KeyValueDTO { Id = s.Id, Value = s.Name, ValueEn=s.NameEn });
        }

    }
}
