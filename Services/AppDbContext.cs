﻿using BlogApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Community> Community { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne()
            .IsRequired()
            .HasForeignKey(p => p.AuthorId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne()
            .IsRequired()
            .HasForeignKey(c => c.AuthorId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne()
            .IsRequired()
            .HasForeignKey(l => l.AuthorId);

        modelBuilder.Entity<Comment>()
            .HasMany(c => c.ChildComments)
            .WithOne(c => c.ParentComment)
            .IsRequired()
            .HasForeignKey(c => c.ParentCommentId);

        modelBuilder.Entity<CommunitySubscriber>()
            .HasKey(cs => new { cs.UserId, cs.CommunityId });

        modelBuilder.Entity<CommunitySubscriber>()
            .HasOne(cs => cs.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(cs => cs.UserId);

        modelBuilder.Entity<CommunitySubscriber>()
            .HasOne(cs => cs.Community)
            .WithMany(c => c.Subscribers)
            .HasForeignKey(cs => cs.CommunityId);

        modelBuilder.Entity<CommunityAdministrator>()
            .HasKey(ca => new { ca.UserId, ca.CommunityId });

        modelBuilder.Entity<CommunityAdministrator>()
            .HasOne(ca => ca.User)
            .WithMany(u => u.ManagedCommunities)
            .HasForeignKey(ca => ca.UserId);

        modelBuilder.Entity<CommunityAdministrator>()
            .HasOne(ca => ca.Community)
            .WithMany(c => c.Administrators)
            .HasForeignKey(ca => ca.CommunityId);
    }
}