using MadPay724.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace MadPay724.Data.DatabaseContext
{
    public class LogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=. ; Initial Catalog=LogMadPay724Db ; Integrated Security=True; MultipleActiveResultSets=True; ");
        }
        public DbSet<ExtendedLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            LogModelBuilderHelper.Build<ExtendedLog>(modelBuilder.Entity<ExtendedLog>());
            modelBuilder.Entity<ExtendedLog>().ToTable("Log");
        }
    }
}
