using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZXSpectrum_MAUI;

public static class DebugMessageParser
{
    private static readonly Dictionary<string, Color> ColorMap = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
    {
        { "red", Colors.Red },
        { "green", Colors.Green },
        { "blue", Colors.Blue },
        { "yellow", Colors.Yellow },
        { "cyan", Colors.Cyan },
        { "magenta", Colors.Magenta },
        { "orange", Colors.Orange },
        { "white", Colors.White },
        { "gray", Colors.Gray },
        { "lightgray", Colors.LightGray },
        { "darkgray", Colors.DarkGray },
        { "lime", Colors.Lime },
        { "purple", Colors.Purple },
        { "pink", Colors.Pink },
    };

    /// <summary>
    /// Parses a message with markup tags and returns a FormattedDebugMessage.
    /// Supported tags:
    /// - &lt;color:colorname&gt;text&lt;/color&gt; or &lt;color:#RRGGBB&gt;text&lt;/color&gt;
    /// - &lt;bold&gt;text&lt;/bold&gt;
    /// - &lt;italic&gt;text&lt;/italic&gt;
    /// - Tags can be nested
    /// </summary>
    public static FormattedDebugMessage Parse(string message)
    {
        var result = new FormattedDebugMessage();
        
        if (string.IsNullOrEmpty(message))
        {
            return result;
        }

        var segments = new List<FormattedTextSegment>();
        var stack = new Stack<FormatContext>();
        var currentContext = new FormatContext();
        
        int pos = 0;
        while (pos < message.Length)
        {
            int tagStart = message.IndexOf('<', pos);
            
            if (tagStart == -1)
            {
                // No more tags, add remaining text
                if (pos < message.Length)
                {
                    string text = message.Substring(pos);
                    if (!string.IsNullOrEmpty(text))
                    {
                        segments.Add(CreateSegment(text, currentContext));
                    }
                }
                break;
            }

            // Add text before the tag
            if (tagStart > pos)
            {
                string text = message.Substring(pos, tagStart - pos);
                segments.Add(CreateSegment(text, currentContext));
            }

            // Find tag end
            int tagEnd = message.IndexOf('>', tagStart);
            if (tagEnd == -1)
            {
                // Malformed tag, treat as text
                segments.Add(CreateSegment(message.Substring(tagStart), currentContext));
                break;
            }

            string tag = message.Substring(tagStart + 1, tagEnd - tagStart - 1);
            pos = tagEnd + 1;

            // Check if it's a closing tag
            if (tag.StartsWith("/"))
            {
                string closingTagName = tag.Substring(1).ToLowerInvariant();
                
                // Pop the context
                if (stack.Count > 0)
                {
                    currentContext = stack.Pop();
                }
            }
            else
            {
                // Opening tag
                stack.Push(currentContext.Clone());
                
                if (tag.Equals("bold", StringComparison.OrdinalIgnoreCase))
                {
                    currentContext.IsBold = true;
                }
                else if (tag.Equals("italic", StringComparison.OrdinalIgnoreCase))
                {
                    currentContext.IsItalic = true;
                }
                else if (tag.StartsWith("color:", StringComparison.OrdinalIgnoreCase))
                {
                    string colorValue = tag.Substring(6).Trim();
                    currentContext.TextColor = ParseColor(colorValue);
                }
            }
        }

        result.Segments = segments;
        return result;
    }

    private static FormattedTextSegment CreateSegment(string text, FormatContext context)
    {
        return new FormattedTextSegment
        {
            Text = text,
            TextColor = context.TextColor,
            IsBold = context.IsBold,
            IsItalic = context.IsItalic
        };
    }

    private static Color ParseColor(string colorValue)
    {
        // Try named colors first
        if (ColorMap.TryGetValue(colorValue, out Color namedColor))
        {
            return namedColor;
        }

        // Try hex color (#RGB or #RRGGBB)
        if (colorValue.StartsWith("#"))
        {
            try
            {
                return Color.FromArgb(colorValue);
            }
            catch
            {
                // Invalid hex color, use default
                return Colors.White;
            }
        }

        // Default to white if color not recognized
        return Colors.White;
    }

    private class FormatContext
    {
        public Color TextColor { get; set; } = Colors.White;
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }

        public FormatContext Clone()
        {
            return new FormatContext
            {
                TextColor = TextColor,
                IsBold = IsBold,
                IsItalic = IsItalic
            };
        }
    }
}
