using Benriya.Utils;
using SkiaSharp;
using System.IO;


namespace Benriya.Core.Classes
{
    public class ImageResize
    {
        private string original_file { get; set; }
        public int size = 1200;
        public int quality = 100;
        public ImageResize(string origin_file)
        {
            original_file = origin_file;
        }

        public bool ToThumnail(int w=0,int h=0)
        {           
            var output_path = $@"{(new DirectoryInfo(original_file).Parent.Parent.FullName)}\{ImagePath.Thumbs}\";
            if (!Directory.Exists(output_path))
                Directory.CreateDirectory(output_path);
            using (var input = File.OpenRead(original_file))
            {
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        int width, height;
                        if (w > 0 || h > 0)
                        {
                            width = w > 0 ? w : size;                            
                            height = h > 0 ? h : original.Height * width / original.Width;
                        }
                        else
                        {
                            if (original.Width <= size || input.ReadByte() < 2_025)
                            {
                                original.Dispose();
                                inputStream.Dispose();
                                input.Dispose();
                                System.IO.File.Move(original_file, $@"{output_path}\{Path.GetFileName(original_file)}");
                                return true;
                            }            
                            if (original.Width > original.Height)
                            {
                                width = size;
                                height = original.Height * size / original.Width;
                            }
                            else
                            {
                                width = original.Width * size / original.Height;
                                height = size;
                            }
                        }

                        using (var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High))
                        {
                            if (resized == null) return false;

                            using (var image = SKImage.FromBitmap(resized))
                            {
                                using (var output = File.OpenWrite($@"{output_path}\{Path.GetFileName(original_file)}"))
                                {
                                    image.Encode(SKEncodedImageFormat.Png,quality).SaveTo(output);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
