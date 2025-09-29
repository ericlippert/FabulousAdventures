// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 4

using System.Drawing.Imaging;

static class Screenshot
{
    static ImageCodecInfo? jpgCodec = null;
    static EncoderParameters? jpgParameters = null;

    static Screenshot()
    {
        jpgCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.MimeType == "image/jpeg");
        if (jpgCodec == null)
            return;
        jpgParameters = new EncoderParameters(1);
        var quality = new EncoderParameter(Encoder.Quality, 75L);
        jpgParameters.Param[0] = quality;
    }

    public static void SaveImage(Image image)
    {
        if (jpgCodec == null) 
            return;
        var name = Path.GetRandomFileName().Replace(".", "") + ".jpg";
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string path = Path.Combine(desktop, name);
        image.Save(path, jpgCodec, jpgParameters);
    }
}