using Microsoft.EntityFrameworkCore;
using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Infrastructure.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // Liste des tables (DbSets) pour ton projet Delivery
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Courier> Couriers => Set<Courier>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuration de l'entité Order (Le pivot central)
        modelBuilder.Entity<Order>(entity => {
            entity.ToTable("Orders");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderDate).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired();

            // Relation avec l'adresse de livraison
            entity.HasOne(o => o.DeliveryAddress)
                  .WithMany()
                  .HasForeignKey(o => o.DeliveryAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation avec le Client
            entity.HasOne<Customer>()
                  .WithMany()
                  .HasForeignKey(o => o.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation avec le Store (Départ)
            entity.HasOne<Store>()
                  .WithMany()
                  .HasForeignKey(o => o.StoreId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 2. Configuration du Store (Point de retrait)
        modelBuilder.Entity<Store>(entity => {
            entity.ToTable("Stores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            
            // Relation avec l'adresse physique du magasin
            entity.HasOne(s => s.Location)
                  .WithMany()
                  .HasForeignKey(s => s.AddressId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 3. Configuration des Produits et Items
        modelBuilder.Entity<Product>(entity => {
            entity.ToTable("Products");
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<OrderItem>(entity => {
            entity.ToTable("OrderItems");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
        });

        // 4. Configuration des Paiements et Reviews
        modelBuilder.Entity<Payment>(entity => {
            entity.ToTable("Payments");
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Review>(entity => {
            entity.ToTable("Reviews");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });
        
        // 5. Configuration des Adresses
        modelBuilder.Entity<Address>(entity => {
            entity.ToTable("Addresses");
            entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
        });
    }
}