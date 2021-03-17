using Microsoft.EntityFrameworkCore;
using SmartContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupContact> GroupContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GroupContact>()
                .HasKey(gc => new { gc.ContactId, gc.GroupId });

            modelBuilder.Entity<GroupContact>()
                .HasOne<Group>(gc => gc.Group)
                .WithMany(g => g.GroupContacts)
                .HasForeignKey(gc => gc.GroupId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<GroupContact>()
                .HasOne<Contact>(gc => gc.Contact)
                .WithMany(c => c.GroupContacts)
                .HasForeignKey(gc => gc.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
