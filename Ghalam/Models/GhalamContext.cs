using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ghalam.Models;

public partial class GhalamContext : DbContext
{
    public GhalamContext()
    {
    }

    public GhalamContext(DbContextOptions<GhalamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LikedStory> LikedStories { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ghalam;Integrated Security=true;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Persian_100_CI_AI");

        modelBuilder.Entity<LikedStory>(entity =>
        {
            entity.HasKey(e => e.PkLikedStory);

            entity.ToTable("LikedStory");

            entity.Property(e => e.PkLikedStory).HasColumnName("PK_LikedStory");
            entity.Property(e => e.FkStory).HasColumnName("FK_Story");
            entity.Property(e => e.FkUser).HasColumnName("FK_User");

            entity.HasOne(d => d.FkStoryNavigation).WithMany(p => p.LikedStories)
                .HasForeignKey(d => d.FkStory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedStory_Story");

            entity.HasOne(d => d.FkUserNavigation).WithMany(p => p.LikedStories)
                .HasForeignKey(d => d.FkUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedStory_user");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.PkStory);

            entity.ToTable("Story");

            entity.Property(e => e.PkStory).HasColumnName("PK_Story");
            entity.Property(e => e.FkWriter).HasColumnName("FK_Writer");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.FkWriterNavigation).WithMany(p => p.Stories)
                .HasForeignKey(d => d.FkWriter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Story_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsWriter).HasColumnName("Is_Writer");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("User_Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
