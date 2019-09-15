using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class AreaMap
    {
        public AreaMap(EntityTypeBuilder<Area> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.HasMany(t => t.Countries).WithOne(u => u.Area).HasForeignKey(x => x.AreaId);
        }
    }
}
