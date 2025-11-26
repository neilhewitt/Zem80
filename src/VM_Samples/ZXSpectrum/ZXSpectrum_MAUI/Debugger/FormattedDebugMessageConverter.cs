using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace ZXSpectrum_MAUI;

public class FormattedDebugMessageConverter
{
    private const string MonospaceFontFamily = "Consolas,Courier New,monospace";

    public static FormattedString Convert(string value, CultureInfo culture)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new FormattedString();
        }

        var parsedMessage = DebugMessageParser.Parse(value);
        var formattedString = new FormattedString();

        foreach (var segment in parsedMessage.Segments)
        {
            var span = new Span
            {
                Text = segment.Text,
                TextColor = segment.TextColor,
                FontAttributes = GetFontAttributes(segment.IsBold, segment.IsItalic),
                FontFamily = MonospaceFontFamily // Ensure monospace font for all spans
            };

            formattedString.Spans.Add(span);
        }

        return formattedString;
    }

    public static string Convert(FormattedString value, CultureInfo culture)
    {
        if (value is not FormattedString formattedString)
        {
            return string.Empty;
        }

        if (formattedString == null || formattedString.Spans.Count == 0)
        {
            return string.Empty;
        }

        var result = new System.Text.StringBuilder();

        foreach (var span in formattedString.Spans)
        {
            var markup = new System.Text.StringBuilder();
            bool needsBold = (span.FontAttributes & FontAttributes.Bold) == FontAttributes.Bold;
            bool needsItalic = (span.FontAttributes & FontAttributes.Italic) == FontAttributes.Italic;
            bool needsColor = span.TextColor != null && span.TextColor != Colors.White;

            // Open tags
            if (needsBold) markup.Append("<bold>");
            if (needsItalic) markup.Append("<italic>");
            if (needsColor)
            {
                // Convert color to markup
                var color = span.TextColor;
                string colorMarkup = ColorToMarkup(color);
                markup.Append($"<color:{colorMarkup}>");
            }

            // Add text
            markup.Append(span.Text);

            // Close tags (in reverse order)
            if (needsColor) markup.Append("</color>");
            if (needsItalic) markup.Append("</italic>");
            if (needsBold) markup.Append("</bold>");

            result.Append(markup);
        }

        return result.ToString();
    }

    public static string ConvertToPlainText(string markedUpValue)
    {
        // remove any markup from the string
        return System.Text.RegularExpressions.Regex.Replace(markedUpValue, "<.*?>", string.Empty);
    }

    private static string ColorToMarkup(Color color)
    {
        // Try to match to named colors first
        if (color == Colors.Red) return "red";
        if (color == Colors.Green) return "green";
        if (color == Colors.Blue) return "blue";
        if (color == Colors.Yellow) return "yellow";
        if (color == Colors.Cyan) return "cyan";
        if (color == Colors.Magenta) return "magenta";
        if (color == Colors.Orange) return "orange";
        if (color == Colors.White) return "white";
        if (color == Colors.Gray) return "gray";
        if (color == Colors.LightGray) return "lightgray";
        if (color == Colors.DarkGray) return "darkgray";
        if (color == Colors.Lime) return "lime";
        if (color == Colors.Purple) return "purple";
        if (color == Colors.Pink) return "pink";

        // For other colors, convert to hex
        int r = (int)(color.Red * 255);
        int g = (int)(color.Green * 255);
        int b = (int)(color.Blue * 255);
        return $"#{r:X2}{g:X2}{b:X2}";
    }


    private static FontAttributes GetFontAttributes(bool isBold, bool isItalic)
    {
        if (isBold && isItalic)
        {
            return FontAttributes.Bold | FontAttributes.Italic;
        }
        else if (isBold)
        {
            return FontAttributes.Bold;
        }
        else if (isItalic)
        {
            return FontAttributes.Italic;
        }
        else
        {
            return FontAttributes.None;
        }
    }
}
