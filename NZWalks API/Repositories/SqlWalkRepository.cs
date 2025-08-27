using Microsoft.EntityFrameworkCore;
using NZWalks_API.Data;
using NZWalks_API.Models.Domain;

namespace NZWalks_API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _context;

        public SqlWalkRepository(NZWalksDbContext context)
        {
            this._context = context;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _context.Walks.AddAsync(walk);
            await SaveChangesAsync();

            return walk;
        }

        public async Task DeleteAsync(Guid id)
        {
            //var walk = await GetByIdAsync(id);
            //if (walk is null) return;

            _context.Walks.Remove(new Walk { Id = id});
            await SaveChangesAsync();
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuer = null, string? sortBy = null, bool? isAscending = true, int pageNumber = 1, int pageSize = 10)
        {
            var walks = _context.Walks
                .Include(d => d.Difficulty)
                .Include(r => r.Region)
                .AsQueryable();


            // Filter
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuer))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(filterQuer));
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (isAscending ?? true) ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                }
                else if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (isAscending ?? true) ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                }
            }

            // Pagination
            var skipWalks = (pageNumber - 1) * pageSize;


            return await walks.Skip(skipWalks).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _context.Walks
                .Include(d => d.Difficulty)
                .Include(r => r.Region)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await GetByIdAsync(id);
            if (existingWalk is null) return null;

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.ImageUrl = walk.ImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await SaveChangesAsync();
            return existingWalk;
        }

        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        
    }
}
