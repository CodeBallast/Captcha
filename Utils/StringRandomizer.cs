using System;
using System.Collections.Generic;
using System.Linq;

namespace CAPTCHA.Utils
{
    /// <summary>
    /// An abstract base class that is able to return a single random character when calling the Generate method.
    /// </summary>
    public abstract class AbstractRandomCharacter
    {
        protected Random Random { get; } = new();
        public abstract char Generate();
    }

    /// <summary>
    /// The Generate method will return a single random character within the range of <see cref="From"/> and <see cref="To"/>.
    /// </summary>
    public class RandomRangeCharacter : AbstractRandomCharacter
    {
        public int From { get; }
        public int To { get; }

        public RandomRangeCharacter(int from, int to)
        {
            From = from;
            To = to;
        }

        public override char Generate()
        {
            return (char)Random.Next(From, To + 1);
        }
    }

    /// <summary>
    /// The Generate method will return a single random character that is contained within <see cref="Chars"/> content.
    /// </summary>
    public class RandomBufferCharacter : AbstractRandomCharacter
    {
        public string Chars { get; }

        public RandomBufferCharacter(string chars)
        {
            Chars = chars;
        }

        public override char Generate()
        {
            var index = Random.Next(0, Chars.Length - 1);
            return Chars[index];
        }
    }

    public enum StringRandomizerType
    {
        Lower,
        Upper,
        Number,
        Special,
        Custom,
        LowerDanish,
        UpperDanish
    }

    /// <summary>
    /// Will generate a random string based on <see cref="Types"/>. The length of the generated string
    /// is determined by <see cref="Size"/>.
    /// </summary>
    public class StringRandomizer
    {
        public HashSet<StringRandomizerType> Types { get; set; } = new()
        {
            StringRandomizerType.Lower, 
            StringRandomizerType.Upper, 
            StringRandomizerType.Number
        };
        public string Custom { get; set; } = null;
        public int Size { get; set; } = 8;

        public string Generate()
        {
            Dictionary<StringRandomizerType, AbstractRandomCharacter> characters =
                new()
                {
                    { StringRandomizerType.Lower, new RandomRangeCharacter(97, 122) },
                    { StringRandomizerType.Upper, new RandomRangeCharacter(65, 90) },
                    { StringRandomizerType.Number, new RandomRangeCharacter(48, 57) },
                    { StringRandomizerType.Special, new RandomBufferCharacter("!\"#%&/\\") },
                    { StringRandomizerType.Custom, new RandomBufferCharacter(Custom) },
                    { StringRandomizerType.LowerDanish, new RandomBufferCharacter("æøå") },
                    { StringRandomizerType.UpperDanish, new RandomBufferCharacter("ÆØÅ") }
                };

            var result = "";
            var random = new Random();
            var types = Types.ToArray();
            for (var i = 0; i < Size; i++)
            {
                var typeIndex = random.Next(0, Types.Count);
                var type = types[typeIndex];
                if (!characters.TryGetValue(type, out var randomizer))
                    throw new ArgumentOutOfRangeException($"StringRandomizerType {type.ToString()} not defined.");
                result += randomizer.Generate();
            }            
            
            return result;
        }
    }
}
