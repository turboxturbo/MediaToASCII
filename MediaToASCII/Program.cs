using System;
using System.IO;
using SkiaSharp;

namespace MediaToASCII
{
    internal class Program
    {
        private const double WIDTH_OFFSET = 1.5;
        private const int MAX_WIDTH = 474;

        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start, then type image path");

            while (true)
            {
                Console.ReadLine();

                var path = PickFile();

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Console.WriteLine("No file selected, try again.");
                    continue;
                }

                var bitmap = SKBitmap.Decode(path);
                if (bitmap == null)
                {
                    Console.WriteLine("Could not decode image, try again.");
                    continue;
                }

                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayScale();

                var converter = new BitmapToASCIIConverter(bitmap);
                var rows = converter.Convert();

                Console.Clear();
                foreach (var row in rows)
                {
                    Console.WriteLine(row);
                }

                Console.SetCursorPosition(0, 0);
            }
        }

        private static string? PickFile()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "osascript",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using var process = System.Diagnostics.Process.Start(psi)!;
            process.StandardInput.WriteLine("POSIX path of (choose file with prompt \"Select an image\" of type {\"public.image\"})");
            process.StandardInput.Close();
            var result = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            return process.ExitCode == 0 ? result : null;
        }

        private static SKBitmap ResizeBitmap(SKBitmap bitmap)
        {
            var newHeight = (int)(bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Width);
            if (bitmap.Width > MAX_WIDTH || bitmap.Height > newHeight)
            {
                var resized = bitmap.Resize(new SKImageInfo(MAX_WIDTH, newHeight), SKFilterQuality.Medium);
                bitmap.Dispose();
                return resized;
            }
            return bitmap;
        }
    }
}
