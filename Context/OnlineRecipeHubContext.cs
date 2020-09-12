using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineRecipeHub.Models;

namespace OnlineRecipeHub.Context
{
    public partial class OnlineRecipeHubContext : DbContext
    {
        public OnlineRecipeHubContext()
        {
        }

        public OnlineRecipeHubContext(DbContextOptions<OnlineRecipeHubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<Level> Level { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<Steps> Steps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=OnlineRecipeHub;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.HasKey(e => e.IngredientId);

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.IngredientName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredients_Recipe");
            });

            modelBuilder.Entity<Level>(entity =>
            {
                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.LevelName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.DateAdded)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image1).HasMaxLength(250);

                entity.Property(e => e.Image2).HasMaxLength(250);

                entity.Property(e => e.Image3).HasMaxLength(250);

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.RecipeTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Recipe)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_Level");
            });

            modelBuilder.Entity<Steps>(entity =>
            {
                entity.Property(e => e.stepsId).HasColumnName("StepsID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.stepName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Steps)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Steps_Recipe");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
