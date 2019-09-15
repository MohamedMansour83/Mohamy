using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ElMaitre.DAL.Data
{
    class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //builder.UseSqlServer("Data Source=MOHAMEDELGENDY\\SQLSERVER2016;Initial Catalog=ELMaitreDb;Integrated Security=True;MultipleActiveResultSets=True",
            builder.UseSqlServer("Data Source=mohamysqldb.cnba20qisdnn.us-west-2.rds.amazonaws.com;Initial Catalog=mohamydb;User Id=mohamysqluser;Password=Ayman1988;MultipleActiveResultSets=True",
                      optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name));

            return new ApplicationDbContext(builder.Options);
        }
    }
}
