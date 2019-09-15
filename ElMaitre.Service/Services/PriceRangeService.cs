using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElMaitre.Extensions;

namespace ElMaitre.Services
{
    public class PriceRangeService : IPriceRangeService
    {
        private IRepository<PriceRange> PriceRangeRepository;

        public PriceRangeService(IRepository<PriceRange> PriceRangeRepository)
        {
            this.PriceRangeRepository = PriceRangeRepository;
        }

        public IEnumerable<PriceRangeDTO> Get()
        {
            return PriceRangeRepository.Get().OrderBy(s=>s.From).Select(s => new PriceRangeDTO
            {
                Id=s.Id,
                From = s.From,
                To = s.To,
            });
        }

    }
}
