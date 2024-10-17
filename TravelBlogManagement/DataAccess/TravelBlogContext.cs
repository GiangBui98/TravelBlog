using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TravelBlogManagement.DataAccess
{
    public partial class TravelBlogContext : DbContext
    {
        public TravelBlogContext()
        {
        }

        public TravelBlogContext(DbContextOptions<TravelBlogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostTag> PostTags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserComment> UserComments { get; set; } = null!;
        public virtual DbSet<UserCommentHistory> UserCommentHistories { get; set; } = null!;
        public virtual DbSet<UserReaction> UserReactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Initial Catalog=TravelBlog;Integrated Security=SSPI;persist security info=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostTags__PostId__29572725");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__PostTags__TagId__2A4B4B5E");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagContent).HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);
            });

            modelBuilder.Entity<UserComment>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserComments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__UserComme__PostI__2E1BDC42");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserComme__UserI__2D27B809");
            });

            modelBuilder.Entity<UserCommentHistory>(entity =>
            {
                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.UserComment)
                    .WithMany(p => p.UserCommentHistories)
                    .HasForeignKey(d => d.UserCommentId)
                    .HasConstraintName("FK__UserComme__UserC__30F848ED");
            });

            modelBuilder.Entity<UserReaction>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserReactions)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__UserReact__PostI__34C8D9D1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserReactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserReact__UserI__33D4B598");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
