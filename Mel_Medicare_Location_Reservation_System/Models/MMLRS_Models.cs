namespace Mel_Medicare_Location_Reservation_System.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MMLRS_Models : DbContext
    {
        public MMLRS_Models()
            : base("name=MMLRS_Models")
        {
        }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Engagement> Engagements { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .Property(e => e.postcode)
                .IsFixedLength();

            modelBuilder.Entity<Branch>()
                .Property(e => e.latitude)
                .HasPrecision(10, 8);

            modelBuilder.Entity<Branch>()
                .Property(e => e.longitude)
                .HasPrecision(11, 8);

            modelBuilder.Entity<Engagement>()
                .Property(e => e.initiator)
                .IsFixedLength();
        }
    }
}
