﻿using System;
using System.Drawing;

namespace TagCloud
{
    public class RandomTheme : ITheme
    {
        private readonly Random random;

        public bool IsChecked { get; set; }

        public RandomTheme()
        {
            random = new Random();
            IsChecked = true;
        }

        public Color BackgroundColor =>
            Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

        public Color GetWordFontColor(WordToken wordToken) =>
            Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
    }
}
