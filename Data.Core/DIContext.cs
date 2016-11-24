using Business.Entities;
using Data.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Core
{
    public class DIContext : DbContext
    {
        public DIContext()
            : base("DIContext")
        {
            Database.SetInitializer<DIContext>(new DbInitializer());
        }

        public DbSet<Blog> BlogSet { get; set; }
        public DbSet<Article> ArticleSet { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new BlogConfiguration());
        }
    }
}
