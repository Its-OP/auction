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
    public DbSet<Stake> Stakes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // var connectionString = (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? string.Empty) == "true"
        //     ? _configuration.GetConnectionString("InContainer")
        //     : _configuration.GetConnectionString("Native");
        
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Native"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.ToTable("Auctions");
            
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.Images).WithOne();
            entity.HasMany(x => x.Stakes).WithOne();
        });
        
        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Images");
            
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        });
        
        modelBuilder.Entity<Stake>(entity =>
        {
            entity.ToTable("Stakes");
            
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