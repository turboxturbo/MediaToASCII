using SkiaSharp;

namespace MediaToASCII
{
    public static class Extensions
    {
        public static void ToGrayScale(this SKBitmap bitmap)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    byte avg = (byte)((pixel.Red + pixel.Green + pixel.Blue) / 3);
                    bitmap.SetPixel(x, y, new SKColor(avg, avg, avg, pixel.Alpha));
                }
            }
        }
    }
}
