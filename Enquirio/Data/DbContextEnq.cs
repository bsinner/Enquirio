using Enquirio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Enquirio.Data {

    public class DbContextEnq : IdentityDbContext<IdentityUser> {
        
        public DbContextEnq (DbContextOptions<DbContextEnq> options) 
            : base(options) { }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Question { get; set; }

    }

}
