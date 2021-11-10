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
        
        public static void KeyDown(int windowsKeyCode)
        {
            IEnumerable<SpectrumKey> spectrumKeys = SpectrumKeysForWindowsKey((WindowsKey)windowsKeyCode);
            SetSpectrumKeyStates(spectrumKeys, KeyState.Down);
        }

        public static void KeyUp(int windowsKeyCode)
        {
            IEnumerable<SpectrumKey> spectrumKeys = SpectrumKeysForWindowsKey((WindowsKey)windowsKeyCode);
            SetSpectrumKeyStates(spectrumKeys, KeyState.Up);
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

        static SpectrumKeyboard()
        {
            Setup();
        }
    }
}
