using System;
using System.Collections.Generic;
using System.Drawing;

namespace CAPTCHA.Utils
{
    public struct RandomFont
    {
        public string Name { get; }
        public Brush Brush { get; }
        public int Size { get; }
        public int Angle { get; }
        public FontStyle Style { get; }

        public RandomFont(string name, Brush brush, int size, int angle, FontStyle style)
        {
            Name = name;
            Brush = brush;
            Size = size;
            Angle = angle;
            Style = style;
        }
    }

    /// <summary>
    /// The <see cref="Generate"/> method will return a random font with a random style, size, color and angle. All the
    /// structural data will be return as <see cref="RandomFont"/> struct.
    /// </summary>
    public class FontRandomizer
    {
        private Random _random = new();

        public List<string> Names { get; set; } = new()
        {
            "Serif",
            "Times New Roman",
            "Georgia",
            "Courier",
            "Sans-serif",
            "Arial",
            "Verdana",
            "Helvetica",
            "Comic Sans Serif"
        };

        public List<Brush> Brushes { get; set; } = new()
        {
            System.Drawing.Brushes.Black,
            System.Drawing.Brushes.Green,
            System.Drawing.Brushes.Blue,
            System.Drawing.Brushes.Red,
            System.Drawing.Brushes.Magenta,
            System.Drawing.Brushes.Maroon
        };

        public int MinFontSize { get; set; } = 16;
        public int MaxFontSize { get; set; } = 30;
        public int MinAngle { get; set; } = -25;
        public int MaxAngle { get; set; } = 25;

        public RandomFont Generate()
        {
            var fontIndex = _random.Next(0, Names.Count);
            var brushIndex = _random.Next(0, Brushes.Count);
            var fontSize = _random.Next(MinFontSize, MaxFontSize);
            var rotation = _random.Next(MinAngle, MaxAngle);
            var style = _random.Next(0, 15);
            return new RandomFont(Names[fontIndex], Brushes[brushIndex], fontSize, rotation, (FontStyle)style);
        }
    }
}
