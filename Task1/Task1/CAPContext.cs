using Microsoft.EntityFrameworkCore;

namespace Task1
{
    public partial class CAPContext : DbContext
    {
        public CAPContext()
        {
        }

        public CAPContext(DbContextOptions<CAPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-AHQRV6P;Initial Catalog=ChamomileAndPartners;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Companies>(entity =>
            {
                entity.Property(e => e.ContractStatus)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.CompanyId)
                    .HasName("IX_CompanyId");

                entity.Property(e => e.Login)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Companies");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
