using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zem80.Core;

namespace ZXSpectrum.VM
{
    public static class SpectrumKeyboard
    {
        private static IDictionary<string, SpectrumKey[]> _map = new Dictionary<string, SpectrumKey[]>();

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
        
        public static void KeyDown(string pcKeyName)
        {
            IEnumerable<SpectrumKey> keys = SpectrumKeysForPCKey(pcKeyName);
            foreach (SpectrumKey key in keys)
            {
                _keyStates[(int)key] = KeyState.Down;
            }
        }

        public static void KeyUp(string pcKeyName)
        {
            IEnumerable<SpectrumKey> keys = SpectrumKeysForPCKey(pcKeyName);
            foreach (SpectrumKey key in keys)
            {
                _keyStates[(int)key] = KeyState.Up;
            }
        }

        public static IEnumerable<SpectrumKey> SpectrumKeysForPCKey(string pcKeyName)
        {
            pcKeyName = pcKeyName.ToUpper();

            List<SpectrumKey> spectrumKeys = new List<SpectrumKey>();
            if (_map.ContainsKey(pcKeyName))
            {
                spectrumKeys.AddRange(_map[pcKeyName]);
            }

            return spectrumKeys;
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
            string[] enumNames = Enum.GetNames(typeof(SpectrumKey));

            // 0 to 9
            for (int i = 0; i < 10; i++)
            {

                _map.Add($"D{i}", new[] { Enum.Parse<SpectrumKey>(enumNames[i + 3]) });
            }

            // A to Z
            for (int i = 'A'; i <= 'Z'; i++)
            {
                string letter = Char.ToString((char)i);
                _map.Add(letter, new[] { Enum.Parse<SpectrumKey>(letter) });
            }

            // modifier keys
            _map.Add("LEFTCTRL", new[] { SpectrumKey.CAPSSHIFT }); // caps shift
            _map.Add("RIGHTCTRL", new[] { SpectrumKey.CAPSSHIFT }); // caps shift
            _map.Add("LEFTSHIFT", new[] { SpectrumKey.SYMBOLSHIFT }); // symbol shift
            _map.Add("RIGHTSHIFT", new[] { SpectrumKey.SYMBOLSHIFT }); // symbol shift

            // other keys
            _map.Add("RETURN", new[] { SpectrumKey.ENTER }); // enter key
            _map.Add("SPACE", new[] { SpectrumKey.SPACE }); // space key

            // overrides / shortcuts
            _map.Add("BACK", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.ZERO }); // BACKSPACE mapped to DELETE (CAPSSHIFT+0)
            _map.Add("LEFT", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.FIVE }); // LEFT arrow
            _map.Add("DOWN", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.SIX }); // DOWN arrow
            _map.Add("UP", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.SEVEN }); // UP arrow
            _map.Add("RIGHT", new[] { SpectrumKey.CAPSSHIFT, SpectrumKey.EIGHT }); // RIGHT arrow
            _map.Add("OEMCOMMA", new[] { SpectrumKey.SYMBOLSHIFT, SpectrumKey.N }); // comma key (SYMBOLSHIFT+N)        
        }

        static SpectrumKeyboard()
        {
            Setup();
        }
    }
}
