using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Enquirio.Models;
using Microsoft.EntityFrameworkCore;

namespace Enquirio.Data {

    public class DbContextEnq : DbContext {
        
        public DbContextEnq (DbContextOptions<DbContextEnq> options) 
            : base(options) { }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Question { get; set; }

//        protected override void OnModelCreating(ModelBuilder builder) { }
    }

}
