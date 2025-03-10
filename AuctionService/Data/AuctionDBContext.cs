﻿using AuctionService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionDBContext(DbContextOptions<AuctionDBContext> options) : DbContext(options)
    {
        public DbSet<Auction> Auctions { get; set; }
        //public DbSet<Item> Items { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Auction>()
        //        .HasOne(a => a.Item)
        //        .WithOne(i => i.Auction)
        //        .HasForeignKey<Item>(i => i.AuctionId);

        //    base.OnModelCreating(modelBuilder);
        //}
    }
    
}
