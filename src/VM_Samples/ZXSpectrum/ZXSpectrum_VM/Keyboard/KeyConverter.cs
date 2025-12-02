using System;
using System.Collections.Generic;
using System.Text;

namespace ZXSpectrum.VM
{
    public static class KeyConverter
    {
        public static WindowsKey WindowsKeyFromVirtualKey(VirtualKey virtualKey)
        {
            // Alphanumeric keys
            if (virtualKey >= VirtualKey.A && virtualKey <= VirtualKey.Z)
                return WindowsKey.A + (virtualKey - VirtualKey.A);
            if (virtualKey >= VirtualKey.Number0 && virtualKey <= VirtualKey.Number9)
                return WindowsKey.D0 + (virtualKey - VirtualKey.Number0);

            // Function keys
            if (virtualKey >= VirtualKey.F1 && virtualKey <= VirtualKey.F12)
                return WindowsKey.F1 + (virtualKey - VirtualKey.F1);

            // Numpad keys
            if (virtualKey >= VirtualKey.NumberPad0 && virtualKey <= VirtualKey.NumberPad9)
                return WindowsKey.NumPad0 + (virtualKey - VirtualKey.NumberPad0);

            // Arrow keys
            switch (virtualKey)
            {
                case VirtualKey.Left: return WindowsKey.Left;
                case VirtualKey.Right: return WindowsKey.Right;
                case VirtualKey.Up: return WindowsKey.Up;
                case VirtualKey.Down: return WindowsKey.Down;
            }

            // Modifier keys
            switch (virtualKey)
            {
                case VirtualKey.Shift: return WindowsKey.LeftShift;
                case VirtualKey.Control: return WindowsKey.LeftCtrl;
                case VirtualKey.Menu: return WindowsKey.LeftAlt;
                case VirtualKey.LeftWindows: return WindowsKey.LWin;
                case VirtualKey.RightWindows: return WindowsKey.RWin;
            }

            // Other common keys
            switch (virtualKey)
            {
                case VirtualKey.Tab: return WindowsKey.Tab;
                case VirtualKey.Enter: return WindowsKey.Enter;
                case VirtualKey.Escape: return WindowsKey.Escape;
                case VirtualKey.Space: return WindowsKey.Space;
                case VirtualKey.Back: return WindowsKey.Back;
                case VirtualKey.Delete: return WindowsKey.Delete;
                case VirtualKey.Insert: return WindowsKey.Insert;
                case VirtualKey.Home: return WindowsKey.Home;
                case VirtualKey.End: return WindowsKey.End;
                case VirtualKey.PageUp: return WindowsKey.PageUp;
                case VirtualKey.PageDown: return WindowsKey.PageDown;
                case VirtualKey.Capital: return WindowsKey.Capital;
                case VirtualKey.Scroll: return WindowsKey.Scroll;
                case VirtualKey.NumLock: return WindowsKey.NumLock;
                case VirtualKey.Print: return WindowsKey.PrintScreen;
                case VirtualKey.Pause: return WindowsKey.Pause;
                case VirtualKey.Apps: return WindowsKey.Apps;
                case VirtualKey.Clear: return WindowsKey.Clear;
                case VirtualKey.Snapshot: return WindowsKey.Snapshot;
                case VirtualKey.Help: return WindowsKey.Help;
                case VirtualKey.Select: return WindowsKey.Select;
                case VirtualKey.Execute: return WindowsKey.Execute;
                case VirtualKey.Separator: return WindowsKey.Separator;
                case VirtualKey.Multiply: return WindowsKey.Multiply;
                case VirtualKey.Add: return WindowsKey.Add;
                case VirtualKey.Subtract: return WindowsKey.Subtract;
                case VirtualKey.Decimal: return WindowsKey.Decimal;
                case VirtualKey.Divide: return WindowsKey.Divide;
                case VirtualKey.Oem1: return WindowsKey.Oem1;
                case VirtualKey.OemPlus: return WindowsKey.OemPlus;
                case VirtualKey.OemComma: return WindowsKey.OemComma;
                case VirtualKey.OemMinus: return WindowsKey.OemMinus;
                case VirtualKey.OemPeriod: return WindowsKey.OemPeriod;
                case VirtualKey.Oem2: return WindowsKey.Oem2;
                case VirtualKey.Oem3: return WindowsKey.Oem3;
                case VirtualKey.Oem4: return WindowsKey.Oem4;
                case VirtualKey.Oem5: return WindowsKey.Oem5;
                case VirtualKey.Oem6: return WindowsKey.Oem6;
                case VirtualKey.Oem7: return WindowsKey.Oem7;
                case VirtualKey.Oem8: return WindowsKey.Oem8;
                case VirtualKey.Oem102: return WindowsKey.Oem102;
            }

            // If not mapped, return WindowsKey.None
            return WindowsKey.None;
        }
    }
}
