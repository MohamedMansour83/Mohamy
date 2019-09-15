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
    public class CountryService : ICountryService
    {
        private IRepository<Province> ProvinceRepository;

        public CountryService(IRepository<Province> ProvinceRepository)
        {
            this.ProvinceRepository = ProvinceRepository;
        }

        public IEnumerable<KeyValueDTO> GetProvinces()
        {

            return ProvinceRepository.Get().Select(s => new KeyValueDTO { Id = s.Id, Value = s.Name, ValueEn = s.NameEn });
        }

       

    }
}
