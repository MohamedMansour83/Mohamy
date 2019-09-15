using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class ReviewMap
    {
        public ReviewMap(EntityTypeBuilder<Review> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Rate).IsRequired();
            entityBuilder.HasMany(t => t.Replies).WithOne(u => u.Review).HasForeignKey(x => x.ReviewId);

        }
    }
}
