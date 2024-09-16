using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JWT_API_BD.Models;

public partial class BasicUserAuthContext : DbContext
{
    public BasicUserAuthContext()
    {
    }

    public BasicUserAuthContext(DbContextOptions<BasicUserAuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<RefreshTokenHistory> RefreshTokenHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.IdNews).HasName("pk_id_news");

            entity.Property(e => e.IdNews).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(70);

            entity.HasOne(d => d.PublishedByNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.PublishedBy)
                .HasConstraintName("fk_published_by");
        });

        modelBuilder.Entity<RefreshTokenHistory>(entity =>
        {
            entity.HasKey(e => e.IdTokenRefresh).HasName("pk_id_refresh_token");

            entity.ToTable("RefreshTokenHistory");

            entity.Property(e => e.IdTokenRefresh).ValueGeneratedNever();
            entity.Property(e => e.RefreshToken).HasMaxLength(200);
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.RefreshTokenHistories)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("fk_id_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("pk_id_user");

            entity.ToTable("User");

            entity.Property(e => e.IdUser).UseIdentityAlwaysColumn();
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
