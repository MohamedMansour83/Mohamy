using ElMaitre.DAL.Data;
using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder.ToTable("IdentityUsers");
            entityBuilder.HasMany(t => t.Reviews).WithOne(u => u.User).HasForeignKey(x => x.UserId);
            entityBuilder.HasMany(t => t.ReceivedDocuments).WithOne(u => u.FromUser).HasForeignKey(x => x.FromUserId);
            entityBuilder.HasMany(t => t.SentDocuments).WithOne(u => u.ToUser).HasForeignKey(x => x.ToUserId);
            entityBuilder.HasMany(t => t.SessionNotes).WithOne(u => u.User).HasForeignKey(x => x.UserId);

        }
    }
}
