using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CAPTCHA.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class Captcha
    {
        private readonly StringRandomizer _stringRandomizer = new();
        private readonly FontRandomizer _fontRandomizer = new();
        private string _answer = null;

        public int Width { get; set; } = 320;
        public int Height { get; set; } = 120;

        // BA: Number of lines to pollute the bitmap.
        public int Pollution { get; set; } = 10;

        public int MarginLeft { get; set; } = 10;

        public int MarginTop { get; set; } = 40;


        // BA: Random Offset on the Y-axis in both the negative and positive direction.
        public int JumpY { get; set; } = 15;

        // BA: Space between each character.
        public int SpaceBetween { get; set; } = 5;

        public HashSet<StringRandomizerType> Types
        {
            get => _stringRandomizer.Types;
            set => _stringRandomizer.Types = value;
        }

        public string CustomCharacters
        {
            get => _stringRandomizer.Custom;
            set => _stringRandomizer.Custom = value;
        }
        public int CharacterSize
        {
            get => _stringRandomizer.Size;
            set => _stringRandomizer.Size = value;
        }

        public int MinFontSize
        {
            get => _fontRandomizer.MinFontSize; 
            set => _fontRandomizer.MinFontSize = value;
        }

        public int MaxFontSize
        {
            get => _fontRandomizer.MaxFontSize;
            set => _fontRandomizer.MaxFontSize = value;
        }

        public int MinAngle
        {
            get => _fontRandomizer.MinAngle;
            set => _fontRandomizer.MinAngle = value;
        }

        public int MaxAngle
        {
            get => _fontRandomizer.MaxAngle;
            set => _fontRandomizer.MaxAngle = value;
        }

        public List<string> FontNames
        {
            get => _fontRandomizer.Names;
            set => _fontRandomizer.Names = value;
        }

        public List<Brush> FontBrushes
        {
            get => _fontRandomizer.Brushes; 
            set => _fontRandomizer.Brushes = value;
        }

        public string Answer => _answer;

        public Captcha()
        {
        }

        public bool CheckAnswer(string text, bool caseInsensitive = true)
        {
            return string.Compare(
                _answer, 
                text,
                caseInsensitive == true ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0;
        }
        
        public void Generate()
        {
            _answer = _stringRandomizer.Generate();
        }
        
        protected string ToBase64(byte[] bytes)
        {
            var result = "data:image/png;base64, " + Convert.ToBase64String(bytes);
            return result;
        }

        protected Brush RandomBrush()
        {
            Random random = new Random();
            int red = random.Next(0, byte.MaxValue + 1);
            int green = random.Next(0, byte.MaxValue + 1);
            int blue = random.Next(0, byte.MaxValue + 1);
            return new SolidBrush(Color.FromArgb(red, green, blue));
        }

        public Bitmap AsBitmap()
        {
            // BA: Calculated positions for each character.
            float x = MarginLeft, y = MarginTop;

            // BA: Make new answer.
            Generate();

            Bitmap bitmap = new(Width, Height);
            using var graphics = Graphics.FromImage(bitmap);

            var random = new Random();

            // BA: Draw random lines to pollute.
            using Pen pen = new(RandomBrush());
            for (var i = 0; i < Pollution; i++)
            {
                graphics.DrawLine(
                    pen,
                    random.Next(0, Width), 
                    random.Next(0, Height), 
                    random.Next(0, Width), 
                    random.Next(0, Height));
                pen.Brush = RandomBrush();
            }

            // BA: Draw each character with different font, rotation, size and style.
            foreach (var c in _answer)
            {
                var randomFont = _fontRandomizer.Generate();
                using var font = new Font(randomFont.Name, randomFont.Size, randomFont.Style);
                var charSize = graphics.MeasureString(c.ToString(), font);
                graphics.ResetTransform();
                graphics.RotateTransform(randomFont.Angle);
                graphics.TranslateTransform(x, y + random.Next(-JumpY, JumpY), MatrixOrder.Append);
                graphics.DrawString(c.ToString(), font, randomFont.Brush, 0, 0);
                x += SpaceBetween + charSize.Width;
            }

            return bitmap;
        }

        public string AsBase64()
        {
            using var stream = new MemoryStream();
            AsBitmap().Save(stream, ImageFormat.Png);
            return ToBase64(stream.ToArray());
        }
    }
}
