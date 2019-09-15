using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class ServiceCategoryMap
    {
        public ServiceCategoryMap(EntityTypeBuilder<ServiceCategory> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.HasMany(t => t.Services).WithOne(u => u.Category).HasForeignKey(x => x.CategoryId);
        }
    }
}
