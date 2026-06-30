using SkiaSharp;

namespace MediaToASCII
{
    internal class BitmapToASCIIConverter
    {
        private readonly char[] _asciitable = { ' ', '.', ',', ':', '+', '*', '?', '%', '$', '#', '@' };
        private readonly SKBitmap _bitmap;

        public BitmapToASCIIConverter(SKBitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public char[][] Convert()
        {
            var result = new char[_bitmap.Height][];
            for (int y = 0; y < _bitmap.Height; y++)
            {
                result[y] = new char[_bitmap.Width];
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    int mapindex = (int)Map(_bitmap.GetPixel(x, y).Red, 0, 255, 0, _asciitable.Length - 1);
                    result[y][x] = _asciitable[mapindex];
                }
            }
            return result;
        }

        private float Map(float value, float start1, float stop1, float start2, float stop2)
        {
            return ((value - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
    }
}
