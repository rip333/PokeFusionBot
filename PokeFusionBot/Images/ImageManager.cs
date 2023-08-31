using PokemonFunctions;
using SixLabors.ImageSharp.Formats.Webp;

namespace Images;

public class ImageManager : IImageManager
{
    private HttpClient _client;

    public ImageManager(HttpClient client)
    {
        _client = client;
    }

    public async Task ConvertPngUrlToWebpAsync(string imageUrl, string outputPath)
    {
        Console.WriteLine($"Converting: {imageUrl}");
        using HttpResponseMessage response = await _client.GetAsync(imageUrl);
        using Stream inputStream = await response.Content.ReadAsStreamAsync();

        // Load the PNG image from the stream
        using Image image = Image.Load(inputStream);

        // Optionally apply any other image transformations here
        // image.Mutate(x => x...);

        // Save as WEBP to the output path
        image.Save(outputPath, new WebpEncoder());
    }
    public async Task<bool> CheckFor404(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);
        return response.StatusCode == System.Net.HttpStatusCode.NotFound;
    }
}
