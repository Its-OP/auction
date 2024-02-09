using domain;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infrastructure;

public class ApplicationDbContext: DbContext, IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.ToTable("Auctions");
            
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.Images).WithOne().OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.Bids).WithOne(x => x.Auction);
        });
        
        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Images");
            
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.HasOne(x => x.Body).WithOne().OnDelete(DeleteBehavior.Cascade).HasForeignKey<ImageBody>(x => x.ImageId);
        });
        
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.ToTable("Bids");
            
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasOne(x => x.User).WithMany();
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => x.Username).IsUnique();
            entity.HasIndex(x => new { x.Username, x.PasswordHash });
        });
    }
}