using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAppADO_ManagmentUniversity.Abstraction;

namespace WpfAppADO_ManagmentUniversity.Context
{
    public partial class ManegerContext : DbContext
    {
        public ManegerContext()
        {
        }

        public ManegerContext(DbContextOptions<ManegerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ManagmentUniversity;Trusted_Connection=True;TrustServerCertificate=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne<Group>(d => d.Group)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.CurGroupId);
            //modelBuilder.Entity<Group>()
            //    .HasMany<Student>(g => g.Students)
            //    .WithOne(s => s.Group)
                
            //    .HasForeignKey(s => s.CurGroupId);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
