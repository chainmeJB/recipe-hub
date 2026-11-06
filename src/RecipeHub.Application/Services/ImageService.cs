using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Interfaces;
using RecipeHub.Infrastructure.Entities;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IRepository<Image> _imageRepository;
        private readonly IMapper mapper;

        public ImageService(IRepository<Image> imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            this.mapper = mapper;
        }

        public async Task<ImageDto> AddImageToRecipeAsync(int recipeId, ImageDto dto, CancellationToken ct = default)
        {
            var image = mapper.Map<Image>(dto);
            image.RecipeId = recipeId;
            image.IsPrimary = false;

            _imageRepository.Add(image);
            await _imageRepository.SaveChangesAsync(ct);

            return mapper.Map<ImageDto>(image);
        }

        public async Task<bool> SetImagePrimaryAsync(int imageId, int recipeId, CancellationToken ct = default)
        {
            var images = await _imageRepository.Query(tracking: true)
                .Where(i => i.RecipeId == recipeId)
                .ToListAsync(ct);

            foreach (var item in images)
            {
                item.IsPrimary = item.Id == imageId;
            }

            if (!images.Any(i => i.Id == imageId))
            {
                return false;
            }

            await _imageRepository.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ImageDto?> GetImageByIdAsync(int id, CancellationToken ct = default)
        {
            var image = await _imageRepository.GetByIdAsync(id, ct);

            if (image == null)
                return null;

            return mapper.Map<ImageDto>(image);
        }

        public async Task<IReadOnlyList<ImageDto>> GetImagesByRecipeIdAsync(int recipeId, CancellationToken ct = default)
        {
            var images = await _imageRepository.Query()
                .AsNoTracking()
                .Where(i => i.RecipeId == recipeId)
                .OrderBy(i => i.Position)
                .ToListAsync(ct);

            return mapper.Map<IReadOnlyList<ImageDto>>(images);
        }

        public async Task UpdateImagesPositionsAsync(int recipeId, IDictionary<int, int> imageIdToPositionMap, CancellationToken ct = default)
        {
            var images = await _imageRepository.Query(tracking: true)
                .Where(i => i.RecipeId == recipeId)
                .ToListAsync(ct);

            foreach (var image in images)
            {
                if (imageIdToPositionMap.TryGetValue(image.Id, out var position))
                {
                    image.Position = position;
                }
            }

            await _imageRepository.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteImageAsync(int id, CancellationToken ct = default)
        {
            var image = await _imageRepository.GetByIdAsync(id, ct);
            if (image == null)
                return false;

            _imageRepository.Remove(image);
            await _imageRepository.SaveChangesAsync(ct);
            return true;
        }
    }
}
