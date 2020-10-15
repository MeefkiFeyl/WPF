namespace CAP
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CAPContext : DbContext
    {
        public CAPContext()
            : base("name=CAPContext")
        {
        }

        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<ContractStatuses> ContractStatuses { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Companies>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Companies>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Companies)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractStatuses>()
                .Property(e => e.ContractStatus)
                .IsUnicode(false);

            modelBuilder.Entity<ContractStatuses>()
                .HasMany(e => e.Companies)
                .WithOptional(e => e.ContractStatuses)
                .HasForeignKey(e => e.ContractStatus);

            modelBuilder.Entity<Users>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
