namespace Images;
public interface IImageManager
{
    Task ConvertPngUrlToWebpAsync(string imageUrl, string outputPath);
}
