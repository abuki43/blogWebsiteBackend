using System;
using mediumBE.Models;
using Microsoft.EntityFrameworkCore;

namespace mediumBE.Data;

public  class MediumContext(DbContextOptions<MediumContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>()
            .HasOne(a => a.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "react" },
            new Tag { Id = 2, Name = "angular" },
            new Tag { Id = 3, Name = "vue" },
            new Tag { Id = 4, Name = "node" },
            new Tag { Id = 5, Name = "deno" }
        );

        base.OnModelCreating(modelBuilder);
    }
    

}
