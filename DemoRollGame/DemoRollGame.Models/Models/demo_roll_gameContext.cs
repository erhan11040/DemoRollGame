using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DemoRollGame.Models.Models
{
    public partial class demo_roll_gameContext : DbContext
    {
        public demo_roll_gameContext()
        {
        }

        public demo_roll_gameContext(DbContextOptions<demo_roll_gameContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMatch> UserMatches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=demo_roll_game;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("match");

                entity.HasIndex(e => e.Name, "UQ__match__72E12F1B6152E0EE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnType("datetime")
                    .HasColumnName("expiresAt");

                entity.Property(e => e.IsComplated).HasColumnName("isComplated");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("startedAt");

                entity.Property(e => e.WinnerName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("winnerName");

                entity.Property(e => e.WinnerRoll).HasColumnName("winnerRoll");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserName, "UQ__user__66DCF95C21DDA672")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<UserMatch>(entity =>
            {
                entity.ToTable("user_match");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsWinner)
                    .IsRequired()
                    .HasColumnName("isWinner")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.JoinedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("joinedAt");

                entity.Property(e => e.MatchId).HasColumnName("matchId");

                entity.Property(e => e.Roll).HasColumnName("roll");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.UserMatches)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_match_fk1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMatches)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_match_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
