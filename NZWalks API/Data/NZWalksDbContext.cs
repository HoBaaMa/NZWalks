using Microsoft.EntityFrameworkCore;
using NZWalks_API.Models.Domain;

namespace NZWalks_API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        DbSet<Difficulty> Difficulties { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<Walk> Walks { get; set; }

    }
}
