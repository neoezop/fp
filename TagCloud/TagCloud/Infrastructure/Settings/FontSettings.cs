﻿using ResultOF;
using System.Drawing;

namespace TagCloud
{
    public class FontSettings
    {
        public string FontName { get; set; }
        public FontStyle Style { get; set; }
        public float DefaultSize { get; set; }
        public float CountMultiplier { get; set; }

        public FontSettings(string fontName, FontStyle style, float size, float multiplier)
        {
            FontName = fontName;
            Style = style;
            DefaultSize = size;
            CountMultiplier = multiplier;
        }

        public static FontSettings GetDefaultSettings() =>
            new FontSettings("Arial", FontStyle.Bold, 12, 1.5f);

        public Result<Font> ValidateFont()
        {
            using (var font = new Font(FontName, 12, FontStyle.Regular))
            {
                return FontName == font.Name ? Result.Ok(font) : Result.Fail<Font>("No such font name");
            }
        }
    }
}
