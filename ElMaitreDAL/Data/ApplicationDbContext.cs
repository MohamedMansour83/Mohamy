using ElMaitre.DAL.Map;
using ElMaitre.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new UserMap(builder.Entity<ApplicationUser>());
            new LawyerMap(builder.Entity<Lawyer>());
            new AreaMap(builder.Entity<Area>());
            new ReviewMap(builder.Entity<Review>());
            new SessionMap(builder.Entity<Session>());
            new LawyerAppointmentMap(builder.Entity<LawyerAppointment>());
            new PriceRangeMap(builder.Entity<PriceRange>());
            new QuestionCategoryMap(builder.Entity<QuestionCategory>());
            new QuestionMap(builder.Entity<Question>());
            new DocumentMap(builder.Entity<Document>());
            new ContractCategoryMap(builder.Entity<ContractCategory>());
            new CountryMap(builder.Entity<Country>());
            new ProvinceMap(builder.Entity<Province>());
            new ServiceMap(builder.Entity<Service>());
            new ServiceCategoryMap(builder.Entity<ServiceCategory>());
            new sessionnoteMap(builder.Entity<SessionNote>());

        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LawyerAppointment> Appointments { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Lawyer> Lawyers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<LawyerSpecialization> LawyerSpecializations { get; set; }
        public DbSet<PriceRange> PriceRanges { get; set; }
        public DbSet<ReviewReply> ReviewReplies { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ContractCategory> ContractCategories { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<LawyerExperience> LawyerExperiences { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<SessionNote> SessionNotes { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServicePrice> ServicePrices { get; set; }
        public DbSet<LawyerService> LawyerServices { get; set; }
        public DbSet<PaymentService> PaymentService { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
