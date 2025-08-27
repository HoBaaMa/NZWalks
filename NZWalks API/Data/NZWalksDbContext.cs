using Microsoft.EntityFrameworkCore;
using NZWalks_API.Models.Domain;

namespace NZWalks_API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data For Difficulties
            // Easy, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("E2975A33-FDB3-4636-B662-0481BC8CC67D"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("4A8FE1B6-A65C-4502-8E7D-8D8160DFCD2E"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("8DAF33FD-AE63-4B9E-9DDA-607430A8921D"),
                    Name = "Hard"
                }
            };

            // Seed "difficulties" to The Database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed Data For Regions

            var regions = new List<Region>()
            {
                new Region
                {
                    Id = Guid.Parse("57DA9F56-AF31-4D60-B753-E90C01374D99"),
                    Name = "Auckland",
                    Code = "AKL",
                    ImageUrl = @"https://www.pexels.com/photo/luxury-apartment-view-over-auckland-skyline-33634443/"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    ImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    ImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    ImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    ImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    ImageUrl = null
                }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
