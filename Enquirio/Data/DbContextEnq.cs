using Enquirio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Enquirio.Data {

    public class DbContextEnq : IdentityDbContext<ApplicationUser> {
        
        public DbContextEnq (DbContextOptions<DbContextEnq> options) 
            : base(options) { }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Question { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            
            // Set answer user fk here to avoid circular on delete cascade
            builder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
