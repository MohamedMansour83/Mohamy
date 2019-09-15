using ElMaitre.DAL.Data;
using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class DocumentMap
    {
        public DocumentMap(EntityTypeBuilder<Document> entityBuilder)
        {
        }
    }
}
