using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace ZXSpectrum_MAUI;

public class FormattedDebugMessage
{
    public List<FormattedTextSegment> Segments { get; set; } = new List<FormattedTextSegment>();
}

public class FormattedTextSegment
{
    public string Text { get; set; } = string.Empty;
    public Color TextColor { get; set; } = Colors.White;
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
}
