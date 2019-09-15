using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class ContractCategoryMap
    {
        public ContractCategoryMap(EntityTypeBuilder<ContractCategory> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.HasMany(t => t.Contracts).WithOne(u => u.Category).HasForeignKey(x => x.CategoryId);
        }
    }
}
