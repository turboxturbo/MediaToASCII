using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaToASCII
{
    internal class Program
    {
        private const double WIDTH_OFFSET = 1.5;
        private const int MAX_WIDTH = 600;

        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images|*.bmp;*.png;*.jpg;*.jpeg" // разрешенные форматы
            };

            Console.WriteLine("Press enter to start");

            while(true) 
            {
                Console.ReadLine();

                if (openFileDialog.ShowDialog() != DialogResult.OK) // Если окно открылось некорректно - выходим из цикла
                {
                    continue;
                }

                Console.Clear(); // очистить консоль

                var bitmap = new Bitmap(openFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayScale();

                var converter = new BitmapToASCIIConverter(bitmap);
                var rows = converter.Convert();

                foreach (var row in rows)
                { 
                    Console.WriteLine(row);
                }

                Console.SetCursorPosition(0, 0);

            }
        }
        

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var maxWidth = MAX_WIDTH;
            var newHeight = bitmap.Height / WIDTH_OFFSET * maxWidth / bitmap.Width;
            if (bitmap.Width > maxWidth || bitmap.Height > newHeight)
            {
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));
            }
            return bitmap;

        }
    }
}

