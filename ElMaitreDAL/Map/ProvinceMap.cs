using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class ProvinceMap
    {
        public ProvinceMap(EntityTypeBuilder<Province> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.HasMany(t => t.Users).WithOne(u => u.Province).HasForeignKey(x => x.ProvinceId);
        }
    }
}
