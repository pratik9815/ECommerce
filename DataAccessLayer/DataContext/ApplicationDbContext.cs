using DataAccessLayer.Models;
using DataAccessLayer.Models.Common;
using DataAccessLayer.Models.Identity;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
                                        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
                                        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
             ICurrentUserService currentUserService):base(options)
        {
            _currentUserService = currentUserService;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                    .HasKey(x => x.Id);

            builder.Entity<ApplicationRole>()
                    .HasKey(x => x.Id);

            builder.Entity<ApplicationRole>()
                    .Property(x => x.RoleType)
                    .IsRequired();

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
                customer.Property(t => t.Name)
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


        }
       //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

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
