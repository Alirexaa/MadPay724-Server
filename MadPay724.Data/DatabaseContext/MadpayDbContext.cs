using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.DatabaseContext
{
    class MadpayDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Sourece=. ; Initial Catalog=MadPay724Db ;Intagrated Security=True; MultipleActiveResultSets=True;  ");
        }
    }
}
