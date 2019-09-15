using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class QuestionMap
    {
        public QuestionMap(EntityTypeBuilder<Question> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Title).IsRequired();
            entityBuilder.HasMany(t => t.Answers).WithOne(u => u.Question).HasForeignKey(x => x.QuestionId);
        }
    }
}
