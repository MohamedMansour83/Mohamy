using ElMaitre.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.DAL.Map
{
    public class LawyerAppointmentMap
    {
        public LawyerAppointmentMap(EntityTypeBuilder<LawyerAppointment> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.HasMany(t => t.Sessions).WithOne(u => u.Appointment).HasForeignKey(x => x.AppointmentId);

        }
    }
}
