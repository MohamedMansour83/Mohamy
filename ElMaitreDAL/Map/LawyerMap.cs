using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class LawyerMap
    {
        public LawyerMap(EntityTypeBuilder<Lawyer> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserId).IsRequired();
            //entityBuilder.Property(t => t.Title).HasMaxLength(200).IsRequired();
            entityBuilder.HasOne(t => t.User).WithOne(u => u.Lawyer).HasForeignKey<Lawyer>(x => x.UserId);
            entityBuilder.HasOne(t => t.Specialization).WithMany(u => u.Lawyers).HasForeignKey(x => x.SpecializationId);
            entityBuilder.HasOne(t => t.Experience).WithMany(u => u.Lawyers).HasForeignKey(x => x.ExperienceId);
            entityBuilder.HasMany(t => t.Reviews).WithOne(u => u.Lawyer).HasForeignKey(x => x.LawyerId);
            entityBuilder.HasMany(t => t.Services).WithOne(u => u.Lawyer).HasForeignKey(x => x.LawyerId);


        }
    }
}
