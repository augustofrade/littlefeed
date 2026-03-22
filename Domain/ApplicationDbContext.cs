using LittleFeed.Domain.Newsletters;
using LittleFeed.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Domain;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public required DbSet<Newsletter> Newsletters { get; set; }
    public required DbSet<Article> Articles { get; set; }
    public required DbSet<UserProfile> UserProfiles { get; set; }
    public required DbSet<NewsletterMember> NewsletterMembers { get; set; }
    public required DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Newsletter
        builder.Entity<Newsletter>()
            .HasIndex(n => n.Slug)
            .IsUnique();
        builder.Entity<Newsletter>()
            .Property(n => n.Slug)
            .HasMaxLength(30)
            .IsRequired();
        builder.Entity<Newsletter>()
            .Property(n => n.Name)
            .HasMaxLength(30)
            .IsRequired();
        builder.Entity<Newsletter>()
            .Property(n => n.Description)
            .HasMaxLength(255);
        builder.Entity<Newsletter>()
            .HasMany<Article>(n => n.Articles)
            .WithOne(a => a.Newsletter)
            .HasForeignKey(a => a.NewsletterId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Newsletter>()
            .HasMany(n => n.Members)
            .WithOne()
            .HasForeignKey(nm => nm.NewsletterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // NewsletterMember
        builder.Entity<NewsletterMember>(b =>
        {
            b.Property(n => n.NewsletterId).IsRequired();
            b.Property(n => n.UserId).IsRequired();
            b.Property(n => n.Role).IsRequired();
            b.HasIndex(n => new { n.NewsletterId, n.UserId });
            b.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Article
        builder.Entity<Article>(b =>
        {
            b.HasIndex(a => a.Slug).IsUnique();
            
            b.Property(a => a.Title)
                .HasMaxLength(50)
                .IsRequired();
            b.Property(a => a.Slug)
                .HasMaxLength(50)
                .IsRequired();
            b.Property(a => a.Excerpt)
                .HasMaxLength(100);

            b.Property(a => a.AuthorId).IsRequired();
            b.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // UserProfile
        builder.Entity<UserProfile>(b =>
        {
            b.Property(u => u.DisplayName).HasMaxLength(40);
            b.Property(u => u.Slug).HasMaxLength(40);
            b.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<UserProfile>(u => u.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<NewsletterSubscription>(b =>
        {
            b.HasKey(s => s.Id);

            b.Property(s => s.Id).ValueGeneratedNever();

            b.HasIndex(s => new { s.NewsletterId, s.UserId })
                .IsUnique()
                .HasFilter("[UserId] IS NOT NULL");

            b.HasIndex(s => new { s.NewsletterId, s.GuestEmail })
                .IsUnique()
                .HasFilter("[GuestEmail] IS NOT NULL");

            b.Ignore(s => s.IsSubscribed);
            
            b.Property(s => s.NewsletterId).IsRequired();

            b.Property(s => s.GuestEmail).HasMaxLength(254);

            b.Property(s => s.SubscribedAt).IsRequired();
            
            b.HasOne<Newsletter>(ns => ns.Newsletter)
                .WithMany(n => n.Subscriptions)
                .HasForeignKey(n => n.NewsletterId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleUpdateDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleUpdateDates()
    {
        var entities = ChangeTracker.Entries().Where(e => e is { Entity: Entity, State: EntityState.Modified or EntityState.Added }).ToList();
        foreach (var entity in entities)
        {
            ((Entity)entity.Entity).ModifiedAt = DateTime.UtcNow;
        }
    }
}