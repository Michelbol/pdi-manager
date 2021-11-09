using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PdiManager.Models;

namespace PdiManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Pdi> Pdis { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
