using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.DatabaseContext
{
    public class MadpayDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=. ; Initial Catalog=MadPay724Db ; Integrated Security=True; MultipleActiveResultSets=True;  ");
        }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.BankCard> BankCards { get; set; }
        public DbSet<Models.Photo> Photos { get; set; }
        public DbSet<Models.Setting> Settings { get; set; }



    }
}
