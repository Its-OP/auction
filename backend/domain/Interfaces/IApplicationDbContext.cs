﻿using Microsoft.EntityFrameworkCore;

namespace domain.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Auction> Auctions { get; }
    public DbSet<Stake> Stakes { get; }
    public DbSet<Image> Images { get; }
    public DbSet<User> Users { get; }
    public Task<int> SaveChangesAsync(CancellationToken token);
}