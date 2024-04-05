using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Conqr.Models
{
    public partial class ConqrContext : DbContext
    {
        public ConqrContext()
        {
        }

        public ConqrContext(DbContextOptions<ConqrContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<CollectionScrolls> CollectionScrolls { get; set; }
        public virtual DbSet<Scrolls> Scrolls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=144.91.71.201;Database=Conqr;UID=PioSun;PWD=pio*123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ReferenceUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<CollectionScrolls>(entity =>
            {
                entity.HasOne(d => d.Collectio)
                    .WithMany(p => p.CollectionScrolls)
                    .HasForeignKey(d => d.CollectioId)
                    .HasConstraintName("FK_Collection_CollectionScrolls");

                entity.HasOne(d => d.Scroll)
                    .WithMany(p => p.CollectionScrolls)
                    .HasForeignKey(d => d.ScrollId)
                    .HasConstraintName("FK_Scrolls_CollectionScrolls");
            });

            modelBuilder.Entity<Scrolls>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
