using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Zem80.Core;

namespace ZXSpectrum.VM
{
    public static class SpectrumKeyboard
    {
        private static IDictionary<int, string> _matrix = new Dictionary<int, string>()
        {
            { 0xF7, "12345" },
            { 0xFB, "QWERT" },
            { 0xFD, "ASDFG" },
            { 0xFE, "^ZXCV" },
            { 0xEF, "09876" },
            { 0xDF, "POIUY" },
            { 0xBF, "#LKJH" },
            { 0x7F, " *MNB" }
        };

        private static KeyState[] _keyStates = new KeyState[95];
        private static IEnumerable<SpectrumKey>[] _keyMap = new IEnumerable<SpectrumKey>[Enum.GetValues(typeof(WindowsKey)).Length];
        
        public static void WpfKeyDown(int windowsKeyCode)
        {
            IEnumerable<SpectrumKey> spectrumKeys = SpectrumKeysForWindowsKey((WindowsKey)windowsKeyCode);
            SetSpectrumKeyStates(spectrumKeys, KeyState.Down);
        }

        public static void WpfKeyUp(int windowsKeyCode)
        {
            IEnumerable<SpectrumKey> spectrumKeys = SpectrumKeysForWindowsKey((WindowsKey)windowsKeyCode);
            SetSpectrumKeyStates(spectrumKeys, KeyState.Up);
        }

        public static void MauiKeyDown(int windowsKeyCode)
        {
            WindowsKey key = ConvertVirtualKeyToWpfKey((VirtualKey)windowsKeyCode);
            WpfKeyDown((int)key);
        }

        public static void MauiKeyUp(int windowsKeyCode)
        {
            WindowsKey key = ConvertVirtualKeyToWpfKey((VirtualKey)windowsKeyCode);
            WpfKeyUp((int)key);
        }

        public static IEnumerable<SpectrumKey> SpectrumKeysForWindowsKey(WindowsKey key)
        {
            return _keyMap[(int)key];
        }

        public static void SetSpectrumKeyStates(IEnumerable<SpectrumKey> keys, KeyState state)
        {
            if (keys == null) return;

            foreach (SpectrumKey key in keys)
            {
                _keyStates[(int)key] = state;
            }
        }

        public static byte GetBitValuesFor(byte rowSelector, byte value)
        {
            if (_matrix.TryGetValue(rowSelector, out string keyRow))
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_keyStates[(int)keyRow[i]] == KeyState.Down)
                    {
                        value = value.SetBit(i, false);
                    }
                }
            }

            return value;
        }

        private static void Setup()
        {
            string[] spectrumKeyNames = Enum.GetNames(typeof(SpectrumKey));

            // 0 to 9
            for (int i = 0; i < 10; i++)
            {
                Map($"D{i}", new[] { Enum.Parse<SpectrumKey>(spectrumKeyNames[i + 3]) });
            }

            // A to Z
            for (int i = 'A'; i <= 'Z'; i++)
            {
                string letter = Char.ToString((char)i);
                Map(letter, new[] { Enum.Parse<SpectrumKey>(letter) });
            }

            // modifier keys
            Map("LEFTCTRL", new[] { SpectrumKey.CAPSSHIFT }); // caps shift
            Map("RIGHTCTRL", new[] { SpectrumKey.CAPSSHIFT }); // caps shift
            Map("LEFTSHIFT", new[] { SpectrumKey.SYMBOLSHIFT }); // symbol shift
            Map("RIGHTSHIFT", new[] { SpectrumKey.SYMBOLSHIFT }); // symbol shift

            // other keys
            Map("RETURN", new[] { SpectrumKey.ENTER }); // enter key
            Map("SPACE", new[] { SpectrumKey.SPACE }); // space key

            // overrides / shortcuts
            Map("BACK", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.ZERO }); // BACKSPACE mapped to DELETE (CAPSSHIFT+0)
            Map("LEFT", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.FIVE }); // LEFT arrow
            Map("DOWN", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.SIX }); // DOWN arrow
            Map("UP", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.SEVEN }); // UP arrow
            Map("RIGHT", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.EIGHT }); // RIGHT arrow
            Map("OEMCOMMA", new[] { SpectrumKey.SYMBOLSHIFT, SpectrumKey.N }); // comma key (SYMBOLSHIFT+N)   
            
            void Map(string name, IEnumerable<SpectrumKey> keys)
            {
                if (Enum.TryParse<WindowsKey>(name, true, out WindowsKey key))
                {
                    _keyMap[(int)key] = keys;
                }
            }
        }

        public static WindowsKey ConvertVirtualKeyToWpfKey(VirtualKey virtualKey)
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

        static SpectrumKeyboard()
        {
            Setup();
        }
    }
}
