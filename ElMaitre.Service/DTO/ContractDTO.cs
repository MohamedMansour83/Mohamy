using ElMaitre.DAL.Models;
using System.Collections;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class ContractDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string NameEn { get; set; }


        public static Contract ToContract(ContractDTO con)
        {

            return new Contract
            {
                Id = con.Id,
                Name = con.Name,
                NameEn = con.NameEn,
                Path = con.Path,
                Description = con.Description,
                DescriptionEn = con.DescriptionEn,
                CategoryId = con.CategoryId
            };
        }

        public static ContractDTO ToContractDTO(Contract con)
        {

            return new ContractDTO
            {
                Id = con.Id,
                Name = con.Name,
                NameEn = con.NameEn,
                DescriptionEn = con.DescriptionEn,
                Path = con.Path,
                Description = con.Description,
                CategoryId=con.CategoryId,
                
            };
        }
    }
}
