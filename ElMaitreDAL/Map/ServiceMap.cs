using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class ServiceMap
    {
        public ServiceMap(EntityTypeBuilder<Service> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.HasMany(t => t.Prices).WithOne(u => u.Service).HasForeignKey(x => x.ServiceId);
            entityBuilder.HasMany(t => t.Lawyers).WithOne(u => u.Service).HasForeignKey(x => x.ServiceId);
        }
    }
}
