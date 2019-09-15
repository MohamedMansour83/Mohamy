using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class CountryMap
    {
        public CountryMap(EntityTypeBuilder<Country> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.HasMany(t => t.Provinces).WithOne(u => u.Country).HasForeignKey(x => x.CountryId);
        }
    }
}
