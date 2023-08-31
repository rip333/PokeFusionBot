using SixLabors.ImageSharp.Formats.Webp;

namespace Images;

public class ImageManager: IImageManager
{
    private HttpClient _client;

    public ImageManager(HttpClient client) {
        _client = client;
    }

    public async Task ConvertPngUrlToWebpAsync(string imageUrl, string outputPath)
    {
        using HttpResponseMessage response = await _client.GetAsync(imageUrl);
        using Stream inputStream = await response.Content.ReadAsStreamAsync();

        // Load the PNG image from the stream
        using Image image = Image.Load(inputStream);

        // Optionally apply any other image transformations here
        // image.Mutate(x => x...);

        // Save as WEBP to the output path
        image.Save(outputPath, new WebpEncoder());
    }
}
