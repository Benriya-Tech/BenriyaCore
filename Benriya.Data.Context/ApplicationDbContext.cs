using System;
using System.Diagnostics;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Toolbelt.ComponentModel.DataAnnotations;

namespace Benriya.Data.Context
{
    public class ApplicationDbContext : DbContext, IStorageContext //DbContext,IStorageContext   //IdentityDbContext<ApplicationUser, ApplicationRole, int>, IStorageContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            Console.WriteLine("ApplicationDbContext start...");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                this.RegisterEntities(modelBuilder);
                modelBuilder.BuildIndexesFromAnnotations();
            }
            catch (Exception) {
                Debug.WriteLine("No execution RegisterEntities() on DbContext...");
            }

        }
    }
}
