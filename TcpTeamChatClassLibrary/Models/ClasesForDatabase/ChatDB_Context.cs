using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace ChatClient.Models
{
    public class ChatDB_Context : DbContext
    {
        public ChatDB_Context() { Database.EnsureCreated(); }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // * пишу на випадок якщо забуду замінити сам
            // для тесту я поверну свій connStr
            // але коли доробимо потрібно буде замінити на:
            // workstation id=ChatClientDB.mssql.somee.com;packet size=4096;user id=ChatClient;pwd=Qwerty_1234!;data source=ChatClientDB.mssql.somee.com;persist security info=False;initial catalog=ChatClientDB;TrustServerCertificate=True
            string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=ChatDB;Trusted_Connection=True;TrustServerCertificate=true;";
            optionsBuilder.UseSqlServer(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Login)
                .IsUnique();

            modelBuilder.Entity<Contact>()
                .HasOne(c=>c.OwnerUser)
                .WithMany(u=>u.OwnContacts)
                .HasForeignKey(u=>u.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.ContactUser)
                .WithMany(u => u.AddedToContacts)
                .HasForeignKey(c => c.ContactUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}