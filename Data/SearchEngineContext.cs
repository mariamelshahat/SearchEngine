using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class SearchEngineContext : DbContext
{
    public SearchEngineContext(DbContextOptions<SearchEngineContext> options)
        : base(options)
    {
    }

    public DbSet<inverted_index> inverted_index { get; set; }
    public DbSet<Urlswithranks> Urlswithranks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<inverted_index>()
            .HasKey(w => new { w.Word, w.FileId });

        modelBuilder.Entity<Urlswithranks>()
            .HasKey(p => p.URL);

        modelBuilder.Entity<inverted_index>()
            .HasOne(w => w.UrlWithRank)
            .WithMany(u => u.InvertedIndices)
            .HasForeignKey(w => w.FileId)
            .HasPrincipalKey(u => u.FileName);

    }
}