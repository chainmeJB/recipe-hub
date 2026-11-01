using RecipeHub.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Application.Interfaces
{
    public interface IImageService
    {
        Task<ImageDto> AddImageToRecipeAsync(int recipeId, ImageDto dto, CancellationToken ct = default);
        Task<bool> SetImagePrimaryAsync(int imageId, int recipeId, CancellationToken ct = default);
        Task<ImageDto?> GetImageByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<ImageDto>> GetImagesByRecipeIdAsync(int recipeId, CancellationToken ct = default);
        Task UpdateImagesPositionsAsync(int recipeId, IDictionary<int, int> imageIdToPositionMap, CancellationToken ct = default);
        Task<bool> DeleteImageAsync(int id, CancellationToken ct = default);
    }
}
