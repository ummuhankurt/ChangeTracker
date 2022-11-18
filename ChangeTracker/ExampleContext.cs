using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ChangeTracker
{
    public partial class ExampleContext : DbContext
    {
        public ExampleContext()
        {
        }

        public ExampleContext(DbContextOptions<ExampleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Parcalar> Parcalars { get; set; }
        public virtual DbSet<UrunParcalar> UrunParcalars { get; set; }
        public virtual DbSet<Urunler> Urunlers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=UMMUHANKURT;Database=Example;Trusted_Connection=True;");
            }
        }
        //Bunu ovverride ettiğimiz zaman,biz ne kadar SaveChanges'ı çağırırsak çağıralım burası tetiklenecek.
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //SaveChanges gerçekleşmeden önce biz araya girerek aşağıdaki komutları gerçekleştirebiliriz.
            //Veritabanında değişiklik yapılmadan önce araya girip operasyon gerçekleştirebiliriz.
            //Override mantığı araya girmemizi sağlar.
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if(entry.State == EntityState.Deleted)
                {
                    Console.WriteLine("Silinme işlemi gerçekleştirildi..");
                }
                else if(entry.State == EntityState.Added)
                {
                    //Burada örneğin eklenen değer için id değeri oluşturulabilir.(otomatik oluşturulmadığı durumlarda).
                    Console.WriteLine("Ekleme işlemi gerçekleştirildi..");
                }
                else
                {
                    //...
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<Parcalar>(entity =>
            {
                entity.ToTable("Parcalar");

                entity.HasIndex(e => e.UrunId, "IX_Parcalar_UrunId");

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.Parcalars)
                    .HasForeignKey(d => d.UrunId);
            });

            modelBuilder.Entity<UrunParcalar>(entity =>
            {
                entity.HasKey(e => new { e.UrunId, e.ParcaId });

                entity.ToTable("UrunParcalar");

                entity.HasIndex(e => e.ParcaId, "IX_UrunParcalar_ParcaId");

                entity.HasOne(d => d.Parca)
                    .WithMany(p => p.UrunParcalars)
                    .HasForeignKey(d => d.ParcaId);

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.UrunParcalars)
                    .HasForeignKey(d => d.UrunId);
            });

            modelBuilder.Entity<Urunler>(entity =>
            {
                entity.HasKey(e => e.UrunId);

                entity.ToTable("Urunler");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
