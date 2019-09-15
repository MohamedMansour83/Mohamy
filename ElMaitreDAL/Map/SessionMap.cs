using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class SessionMap
    {
        public SessionMap(EntityTypeBuilder<Session> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(x => x.SessionId).HasDefaultValueSql("NEWID()");
            entityBuilder.HasMany(t => t.Notes).WithOne(u => u.Session).HasForeignKey(x => x.SessionId);

            //entityBuilder.Property(t => t.UserId).IsRequired();
        }
    }
}
