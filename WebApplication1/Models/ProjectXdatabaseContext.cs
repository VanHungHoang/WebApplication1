using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplication1.ViewModels;

namespace WebApplication1.Models
{
    public partial class ProjectXdatabaseContext : DbContext
    {
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<AuthorVM> AuthorVMs { get; set; }
        public virtual DbSet<Chapters> Chapters { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Stories> Stories { get; set; }
        public virtual DbSet<StoryGenre> StoryGenre { get; set; }
        public virtual DbSet<UserWatch> UserWatch { get; set; }

        public ProjectXdatabaseContext(DbContextOptions<ProjectXdatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasKey(e => e.AuthorId)
                    .HasName("PK__Authors__70DAFC34CC976CBD");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Chapters>(entity =>
            {
                entity.HasKey(e => e.ChapterId)
                    .HasName("PK__Chapters__0893A36A74889052");

                entity.Property(e => e.ChapterContent).IsRequired();

                entity.Property(e => e.ChapterTitle).HasMaxLength(50);

                entity.Property(e => e.LastEditedDate).HasColumnType("datetime");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UploadedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_StoryChapters");
            });

            modelBuilder.Entity<Genres>(entity =>
            {
                entity.HasKey(e => e.GenreId)
                    .HasName("PK__Genres__0385057EB81E776F");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PK__Reviews__74BC79CEF13AD23C");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastEditedDate).HasColumnType("datetime");

                entity.Property(e => e.ReviewContent).IsRequired();

                entity.Property(e => e.ReviewTitle).HasMaxLength(50);

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Stories>(entity =>
            {
                entity.HasKey(e => e.StoryId)
                    .HasName("PK__Stories__3E82C04809D21F26");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastEditedDate).HasColumnType("datetime");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StoryDescription).IsRequired();

                entity.Property(e => e.StoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Stories)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_AuthorStories");
            });

            modelBuilder.Entity<StoryGenre>(entity =>
            {
                entity.HasKey(e => new { e.StoryId, e.GenreId })
                    .HasName("PK__StoryGen__CEBA901F7B644BAD");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.StoryGenre)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_StoriesGenre");

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.StoryGenre)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_StoryGenres");
            });

            modelBuilder.Entity<UserWatch>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.StoryId })
                    .HasName("PK__UserWatc__8460E0480C17961F");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.UserWatch)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_UsersWatch");
            });
        }
    }
}