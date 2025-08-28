using NZWalks_API.Models.Domain;

namespace NZWalks_API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
