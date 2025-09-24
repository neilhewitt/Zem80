using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace ZexNext.Core
{
    public class TestSet
    {
        private ushort? _patchAddress;
        private byte[] _patchData;
        private bool _hasPatch;

        public string Name { get; private set; }
        public IReadOnlyList<Test> Tests { get; private set; }
        public bool ContainsMemoryPatch => _hasPatch;
        
        public void PatchMemory(Action<ushort, byte[]> memoryPatcher)
        {
            if (_hasPatch)
            {
                memoryPatcher(_patchAddress.Value, _patchData);
            }
        }

        private IReadOnlyList<Test> Setup(string dataPath)
        {
            if (!File.Exists(dataPath))
            {
                throw new FileNotFoundException("Specified file could not be found at path \"" + dataPath + "\".");
            }
            else
            {
                IEnumerable<string> file = GetLines(dataPath);
                List<Test> tests = new List<Test>();

                string testName = String.Empty;
                Test test = null;
                TestState before = null, after = null;
                foreach (string line in file)
                {
                    if (line.StartsWith("P:") && !_hasPatch)
                    {
                        _hasPatch = true; // can only be one patch
                        string[] patchParts = line.Substring(2).Split("|");
                        _patchAddress = ushort.Parse(patchParts[0], System.Globalization.NumberStyles.HexNumber);
                        string[] patchDataParts = patchParts[1].Split(',');
                        _patchData = new byte[patchDataParts.Length];
                        for (int i = 0; i < patchDataParts.Length; i++)
                        {
                            _patchData[i] = byte.Parse(patchDataParts[i], System.Globalization.NumberStyles.HexNumber);
                        }
                    }
                    else if (line.StartsWith("T:"))
                    {
                        // it's a section title
                        string[] testNameParts = line.Split('|');
                        testName = testNameParts[0].Substring(2);
                        byte mask = 0xFF;
                        if (testNameParts.Length > 1)
                        {
                            mask = byte.Parse(testNameParts[1], System.Globalization.NumberStyles.HexNumber);
                        }
                        test = new Test(testName, mask);
                        tests.Add(test);
                    }
                    else
                    {
                        string left = line.Substring(0, line.IndexOf(']') + 1);
                        string mid = line.Substring(left.Length + 1, line.IndexOf('[', left.Length + 1));
                        string right = line.Substring(line.IndexOf('[', left.Length + 1));

                        string mode = left.Substring(0, left.IndexOf(':'));
                        string mnemonic = left.Substring(3, (left.IndexOf('"', 4) - 3));
                        string opcode = left.Substring(mode.Length + mnemonic.Length + 4);

                        string[] registers = mid.Split(',');
                        string[] opcodeParts = opcode.Replace("]", null).Replace("[", null).Split(',');
                        string[] dataParts = right.Substring(1, right.Length - 2).Split(',');

                        byte[] opcodeBytes = new byte[opcodeParts.Length];
                        for (int i = 0; i < opcodeBytes.Length; i++)
                        {
                            opcodeBytes[i] = byte.Parse(opcodeParts[i], System.Globalization.NumberStyles.HexNumber);
                        }

                        byte[] dataBytes = new byte[dataParts.Length];
                        for (int i = 0; i < dataBytes.Length; i++)
                        {
                            dataBytes[i] = byte.Parse(dataParts[i], System.Globalization.NumberStyles.HexNumber);
                        }

                        TestState state = new TestState(
                            opcodeBytes,
                            mnemonic,
                            dataBytes,
                            ushort.Parse(registers[0], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[1], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[2], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[3], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[4], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[5], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[6], System.Globalization.NumberStyles.HexNumber),
                            ushort.Parse(registers[7], System.Globalization.NumberStyles.HexNumber)
                            );

                        if (mode == "I")
                        {
                            before = state;
                        }
                        else
                        {
                            after = state;
                        }

                        if (before != null && after != null)
                        {
                            test.Add(new TestCycle(test, before, after, line));
                            before = null;
                            after = null;
                        }
                    }
                }

                return tests;
            }
        }

        private IEnumerable<string> GetLines(string dataPath)
        {

            if (Path.GetExtension(dataPath) == ".zip")
            {
                // unzip file into memory here
                ZipArchive zip = ZipFile.OpenRead(dataPath);
                ZipArchiveEntry zxl = zip.GetEntry(Path.GetFileNameWithoutExtension(dataPath) + ".zxl");
                if (zxl == null)
                {
                    // bad zip format (should be just the .zxl file with the same name as the archive)
                    throw new Exception("Zip file was not in the correct format"); // TODO: custom exceptions
                }
                else
                {
                    zip.ExtractToDirectory(Path.GetDirectoryName(dataPath), true);
                    dataPath = Path.ChangeExtension(dataPath, ".zxl");
                }
            }
            
            string[] lines = File.ReadAllLines(dataPath);
            return lines;
        }

        public TestSet(string name, string dataPath)
        {
            Name = name;
            Tests = Setup(dataPath);
        }
    }
}
