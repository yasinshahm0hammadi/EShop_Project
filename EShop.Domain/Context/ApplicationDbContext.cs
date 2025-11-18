using EShop.Domain.Entities.Account.Role;
using EShop.Domain.Entities.Account.User;
using EShop.Domain.Entities.Contact;
using EShop.Domain.Entities.Contact.Ticket;
using EShop.Domain.Entities.Product;
using EShop.Domain.Entities.Site;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    #region Account

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    #endregion

    #region Company Information

    public DbSet<SiteSetting> SiteSettings { get; set; }
    public DbSet<AboutUs> AboutUs { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<Question> Questions { get; set; }

    #endregion

    #region Contact Us

    public DbSet<ContactUs> Contacts { get; set; }

    #region Ticket

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketMessage> TicketMessages { get; set; }

    #endregion

    #endregion

    #region Site Images

    #region Slider

    public DbSet<Slider> Sliders { get; set; }

    #endregion

    #region Site Banners

    public DbSet<SiteBanner> SiteBanners { get; set; }

    #endregion

    #endregion

    #region Product

    public DbSet<Product> Products { get; set; }

    #region Product Category

    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }

    #endregion

    #region Product Color

    public DbSet<ProductColor> ProductColors { get; set; }

    #endregion

    #region

    public DbSet<ProductFeature> ProductFeatures { get; set; }

    #endregion

    #endregion

    #region OnModelCreating

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}