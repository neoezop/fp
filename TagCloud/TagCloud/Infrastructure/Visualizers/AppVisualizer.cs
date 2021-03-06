﻿using ResultOF;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class AppVisualizer : IVisualizer
    {
        private readonly Reader reader;
        private readonly IExtractor extractor;
        private readonly ILayouter layouter;
        private readonly IParser[] parsers;
        private readonly IFilter[] filters;
        private readonly FontSettings fontSettings;
        private readonly ImageSettings imageSettings;
        private readonly ImageHolder imageHolder;
        private readonly ITheme[] themes;

        public AppVisualizer(Reader reader, IExtractor extractor, ILayouter layouter,
            IParser[] parsers, IFilter[] filters, FontSettings fontSettings, ITheme[] themes,
            ImageSettings imageSettings, ImageHolder imageHolder)
        {
            this.reader = reader;
            this.extractor = extractor;
            this.layouter = layouter;
            this.parsers = parsers;
            this.filters = filters;
            this.fontSettings = fontSettings;
            this.imageSettings = imageSettings;
            this.themes = themes;
            this.imageHolder = imageHolder;
        }

        public void Visualize()
        {
            GetWordTokens()
                .Then(DrawWordTokens);
            imageHolder.UpdateUi();
            layouter.Reset();
        }

        private Result<WordToken[]> GetWordTokens()
        {
            var text = reader.ReadTextFromFile().GetValueOrThrow();

            return extractor.ExtractWords(text)
                .Then(FilterWords)
                .Then(ParseWords)
                .Then(WordTokenizer.TokenizeWithNoSpeechPart);
        }

        private Result<string[]> FilterWords(string[] words)
        {
            var filteredWords = words;
            filters
                .Where(filter => filter.IsChecked)
                .ForEach(filters => filteredWords = filters.FilterWords(filteredWords).GetValueOrThrow());
            return filteredWords;
        }

        private Result<string[]> ParseWords(string[] words)
        {
            var parsedWords = words;
            parsers
                .Where(parser => parser.IsChecked)
                .ForEach(parser => parsedWords = parser.ParseWords(parsedWords).GetValueOrThrow());
            return parsedWords;
        }

        private Result<None> DrawWordTokens(WordToken[] wordTokens)
        {
            var theme = themes.First(t => t.IsChecked);
            using (var graphics = imageHolder.StartDrawing())
            {
                graphics.FillRectangle(new SolidBrush(theme.BackgroundColor), 0, 0, imageSettings.Width, imageSettings.Height);
                foreach (var wordToken in wordTokens)
                    DrawWord(wordToken, graphics, theme);
            }
            return Result.Ok();
        }

        private void DrawWord(WordToken wordToken, Graphics graphics, ITheme theme)
        {
            var font = new Font(fontSettings.FontName, GetFontSize(wordToken), fontSettings.Style);
            var wordRectangle = layouter.PutNextRectangle(GetWordSize(wordToken, graphics, font));
            graphics.DrawString(wordToken.Value, font, new SolidBrush(theme.GetWordFontColor(wordToken)),
                wordRectangle.GetValueOrThrow());
        }

        private float GetFontSize(WordToken wordToken) =>
             fontSettings.DefaultSize + wordToken.Count * fontSettings.CountMultiplier;

        private static SizeF GetWordSize(WordToken wordToken, Graphics graphics, Font font) =>
            graphics.MeasureString(wordToken.Value, font);
    }
}
