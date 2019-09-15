using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class QuestionCategoryMap
    {
        public QuestionCategoryMap(EntityTypeBuilder<QuestionCategory> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Title).IsRequired();
            entityBuilder.HasMany(t => t.Questions).WithOne(u => u.QuestionCategory).HasForeignKey(x => x.CategoryId);
        }
    }
}
