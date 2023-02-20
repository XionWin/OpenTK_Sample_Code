using OpenTK.Windowing.Common.Input;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Runtime.InteropServices;

namespace Extension;
public static class WindowExtension
{
    public static WindowIcon CreateWindowIcon(string iconPath)
    {
        using var image = (Image<Rgba32>)SixLabors.ImageSharp.Image.Load(Configuration.Default, iconPath);
        return image.GetPixelData() is var pixelSpan &&
        MemoryMarshal.AsBytes(pixelSpan).ToArray() is byte[] imageBytes ?
        new WindowIcon(new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, imageBytes)) :
        throw new Exception("CreateWindowIcon error");
    }

    public static Span<TPixel> GetPixelData<TPixel>(this Image<TPixel> image)
        where TPixel : unmanaged, IPixel<TPixel> =>
        image.Frames.RootFrame.Size() is var size &&
            new TPixel[size.Width * size.Height] is var pixelSpan ?
            pixelSpan.With(x => image.CopyPixelDataTo(x)) :
            throw new Exception("CreateWindowIcon error");
}
