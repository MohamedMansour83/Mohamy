using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class sessionnoteMap
    {
        public sessionnoteMap(EntityTypeBuilder<SessionNote> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            //entityBuilder.Property(t => t.UserId).IsRequired();
        }
    }
}
