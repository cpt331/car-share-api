using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarShareApi.Models.Repositories.Data
{
    public class CarShareContext : DbContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Car> Cars { get; set; }

        public CarShareContext() : base("CarShareContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}