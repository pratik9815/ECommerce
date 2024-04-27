using DataAccessLayer.Models;
using DataAccessLayer.Models.Common;
using DataAccessLayer.Models.Identity;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
                                        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
                                        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;
        public string connectionstring;
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
             ICurrentUserService currentUserService, IConfiguration configuration):base(options)
        {
            _currentUserService = currentUserService;
            _configuration = configuration;
            connectionstring = _configuration.GetConnectionString("DefaultConnection");
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
            
        //    optionsBuilder.UseSqlServer(connectionstring);
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                    .HasKey(x => x.Id);

            builder.Entity<ApplicationRole>()
                    .HasKey(x => x.Id);

            //builder.Entity<ApplicationRole>()
            //        .Property(x => x.RoleType)
            //        .IsRequired();

            builder.Entity<ApplicationUserRole>()
                    .ToTable("AspNetUserRoles");


            //Many to many relationship between applicationUser and applicationRole
            //The Entity<ApplicationUserRole> method is used to configure the ApplicationUserRole entity.
            builder.Entity<ApplicationUserRole>(userRole =>
            {
                // setting primary key
                // This primary key is composite key that consist of userId and roleId
                userRole.HasKey(aur => new
                {
                    aur.UserId,
                    aur.RoleId
                });
                //Here we are configuring the relationship
                userRole.HasOne(aur => aur.User)
                        .WithMany(au => au.UserRoles)
                        .HasForeignKey(aur => aur.UserId);

                userRole.HasOne(aur => aur.Role)
                        .WithMany(ar => ar.UserRoles)
                        .HasForeignKey(aur => aur.RoleId);
            });

            //This code is defining a many-to-many relationship between the ApplicationRole
           // and ApplicationUser entities using the ApplicationUserRole entity as the join table.
            builder.Entity<ApplicationRole>()
                    .HasMany(ar => ar.Users)
                    .WithMany(au => au.Roles)
                    .UsingEntity<ApplicationUserRole>(aur =>
                    {
                        aur.HasOne(aur => aur.Role)
                            .WithMany(ar => ar.UserRoles);

                        aur.HasOne(aur => aur.User)
                            .WithMany(au => au.UserRoles);
                    });

            

            //Setting up customer
            builder.Entity<Customer>(customer =>
            {
                customer.HasKey(c => c.Id);
                customer.Property(t => t.FullName)
                 .HasMaxLength(50)
                 .IsRequired();
                customer.Property(t =>t.Address)
                        .HasMaxLength(200)
                        .IsRequired();
            });
            //Setting up category
            builder.Entity<Category>(category =>
            {
                category.HasKey(c => c.Id);
                category.Property(t => t.CategoryName)
                 .HasMaxLength(50)
                 .IsRequired();
                category.Property(t => t.Description)
                        .HasMaxLength(500)
                        .IsRequired();
            });

            //Setting up product and productcategory 

            builder.Entity<Product>(product =>
            {
                product.HasKey(c => c.Id);
                product.Property( p => p.Name)
                        .HasMaxLength(50)
                        .IsRequired();
                product.Property(p => p.Price)
                        .IsRequired();

            });
            builder.Entity<ProductCategory>(productCategory => 
            {
                productCategory.HasKey(c => c.Id);
            });

            builder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            builder.Entity<ProductCategory>()
                 .HasOne(pc => pc.Category)
                 .WithMany(p => p.ProductCategories)
                 .HasForeignKey(pc => pc.CategoryId);


            //defining primary key
            builder.Entity<OrderDetails>(o =>
            {
                o.HasKey(c => c.Id);
               
            });

            builder.Entity<Order>(o =>
            {
                o.HasKey(c => c.Id);
                o.Property(t => t.ShippingAddress)
               .HasMaxLength(50)
               .IsRequired();

                o.Property(t => t.OrderEmail)
              .HasMaxLength(50)
              .IsRequired();
            });

            builder.Entity<ProductSubCategory>(psc =>
            {
                psc.HasKey(c => new
                {
                    c.CategoryId,
                    c.SubCategoryId,
                    c.ProductId
                });
            });

            //configuring product and order details relationship

            builder.Entity<OrderDetails>(o =>
            {
               o.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);
            });
            builder.Entity<OrderDetails>(o =>
            {
                o.HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);
            });

            builder.Entity<ProductImage>()
                .HasKey(c => c.Id);

            builder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId); 

            builder.Entity<SubCategory>()
                .HasKey(a => a.Id);



            builder.Entity<SystemAccessLog>()
                .HasKey(a => a.Id);

            builder.Entity<Order>()
                   .HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId);   

            builder.Entity<Product>()
                .HasMany(p => p.ProductReviews)
                .WithOne(pr => pr.Product)
                .HasForeignKey(pr => pr.ProductId);

            //category and subcategory one to may relationship
            builder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId);

            //productsubcategory and product one to many relationship
            builder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.Product)
                .WithMany(p => p.ProductSubCategories)
                .HasForeignKey(psc => psc.ProductId);

            //productsubcategory nad subcategory relationship
            builder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.SubCategory)
                .WithMany(sc => sc.ProductSubCategories)
                .HasForeignKey(psc => psc.SubCategoryId);

            // productsubcategory and subcategory relationship


        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SystemAccessLog> SystemAccessLogs { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<OrderActivityLog> OrderActivityLogs { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }   
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //Using the change tracker
            //Get all the entities that inherit from AuditableEntity and have a state of Added or Modifie
            //for each entity we will set the audit properties
            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                //Here using the switch 
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedBy = _currentUserService.UserId;
                        entry.Entity.DeletedDate = DateTime.UtcNow;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
