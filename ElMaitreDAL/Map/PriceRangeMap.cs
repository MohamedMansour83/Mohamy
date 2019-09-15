using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class PriceRangeMap
    {
        public PriceRangeMap(EntityTypeBuilder<PriceRange> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
        }
    }
}
