using EventShopApp.Enums;
using EventShopApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<ArrangementItem> ArrangementItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Seed Flowers
            modelBuilder.Entity<Flower>().HasData(
                new Flower
                {
                    Id = 1,
                    FlowerType = "Rose",
                    Price = 2.99m,
                    Description = "A beautiful red rose.",
                    FlowerQuantity = 100,
                    FlowerImageUrl = "https://www.google.com/search?sca_esv=4804c780a61df574&rlz=1C1CSMH_deBG1019BG1019&sxsrf=ADLYWILL-fEBWMKJ8DEd93sgIi-ZMaaubw:1731505504049&q=rose&udm=2&fbs=AEQNm0D0mdjV9iZmrIToWZfLy6hjiHLZlz0gO0cW40eqjD3LgTC_9I288s3dQhxfUDXs5Fh64FGxavo5glsqTygQ17zo5u5z-gmkJwHk96CuJucXHmdluPwYGcIpyynasv9IftnWJq-CfxpS_cad0RJd64zY0_BoK5ArRwSPBg01jRrMOCRHwSALX6-XKMwhPRWNubgHCdfCPqfrmwSM-EXYGxVfKhnPsPbd-f0c-EuCDsO_bpwPW8w&sa=X&ved=2ahUKEwiB9PrTuNmJAxVLS_EDHeVdDX0QtKgLegQIERAB&biw=1920&bih=911&dpr=1#vhid=7P8wcguiNtDg_M&vssid=mosaic"
                },
                new Flower
                {
                    Id = 2,
                    FlowerType = "Tulip",
                    Price = 1.99m,
                    Description = "A charming yellow tulip.",
                    FlowerQuantity = 50,
                    FlowerImageUrl = "https://s3.amazonaws.com/cdn.tulips.com/images/large/Timeless-Tulip.jpg"
                }
            );

            // Seed Arrangement Items
            modelBuilder.Entity<ArrangementItem>().HasData(
                new ArrangementItem
                {
                    Id = 1,
                    ArrangementItemType = "Birthday Bouquet",
                    Price = 19.99m,
                    Description = "A colorful bouquet perfect for birthdays.",
                    ArrangementItemsQuantity = 20,
                    ArrangementItemImageUrl = "https://example.com/images/birthday_bouquet.jpg"
                },
                new ArrangementItem
                {
                    Id = 2,
                    ArrangementItemType = "Wedding Arrangement",
                    Price = 29.99m,
                    Description = "Elegant flowers for weddings.",
                    ArrangementItemsQuantity = 15,
                    ArrangementItemImageUrl = "https://example.com/images/wedding_arrangement.jpg"
                }
            );

            // Add the first employee record
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Slavcho",
                    Surname = "Vakov",
                    PhoneNumber = "+359893540139",
                    Email = "vakovslavcho@gmail.com",
                    Role = EmployeeRole.Owner,
                    HireDate = DateTime.UtcNow,
                    IsFired = false
                }
            );

            // Seed the AspNetUsers table
            var hasher = new PasswordHasher<IdentityUser>();
            var ownerUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(), // Unique ID for the user
                UserName = "vakovslavcho@gmail.com",
                NormalizedUserName = "VAKOVSLAVCHO@GMAIL.COM",
                Email = "vakovslavcho@gmail.com",
                NormalizedEmail = "VAKOVSLAVCHO@GMAIL.COM",
                EmailConfirmed = true
            };
            ownerUser.PasswordHash = hasher.HashPassword(ownerUser, "Owner@123");

            modelBuilder.Entity<IdentityUser>().HasData(ownerUser);
        }
    }
}

    

