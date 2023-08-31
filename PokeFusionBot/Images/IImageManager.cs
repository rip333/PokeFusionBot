using PokemonFunctions;

namespace Images;
public interface IImageManager
{
    Task ConvertPngUrlToWebpAsync(string imageUrl, string outputPath);
    Task<bool> CheckFor404(string url);
}
