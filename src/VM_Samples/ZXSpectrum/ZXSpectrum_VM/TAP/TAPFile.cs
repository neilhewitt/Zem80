using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZXSpectrum.VM.TAP
{
    public class TAPFile
    {
        private List<TAPBlock> _blocks = new List<TAPBlock>();

        public string Path { get; private set; }
        public TAPHeader Header { get; private set; }

        public IReadOnlyList<TAPBlock> Blocks => _blocks;

        private void ParseData(byte[] fileData)
        {
            Header = new TAPHeader(fileData[0..19]);
            ExtractBlocks(fileData[19..]);
        }

        private void ExtractBlocks(byte[] allBlockData)
        {
            int index = 2;
            while (true)
            {
                ushort blockSize = (ushort)((byte)allBlockData[index] + ((byte)allBlockData[++index] * 256));
                TAPBlock block = new TAPBlock((ushort)(blockSize - 2), allBlockData[new Range(++index, index + blockSize)]);
                index = index + blockSize;
                _blocks.Add(block);

                if (index >= allBlockData.Length) break;
            }
        }

        public TAPFile(string path)
        {
            if (File.Exists(path))
            {
                Path = path;
                byte[] fileData = File.ReadAllBytes(Path);
                ParseData(fileData);
            }
        }

        public TAPFile(byte[] data)
        {
            Path = ".";
            ParseData(data);
        }
    }

    public class TAPHeader
    {
        private byte[] _headerData;

        public TAPType Type { get; private set; }
        public string Filename { get; private set; }
        public ushort LengthInBytes { get; private set; }
        public ushort Parameter1 { get; private set; }
        public ushort Parameter2 { get; private set; }

        private void ParseHeader()
        {
            Type = (TAPType)(int)((byte)_headerData[2]);
            Filename = System.Text.ASCIIEncoding.Default.GetString(_headerData[4..13]).TrimEnd();
            LengthInBytes = doubleByteValue(14);
            Parameter1 = doubleByteValue(16);
            Parameter2 = doubleByteValue(18);

            ushort doubleByteValue(int index)
            {
                return (ushort)((byte)_headerData[index] + ((byte)_headerData[index+1] * 256));
            }
        }


        public TAPHeader(byte[] headerData)
        {
            try
            {
                _headerData = headerData;
                ParseHeader();
            }
            catch
            {
            }
        }
    }

    public class TAPBlock
    {
        public ushort SizeInBytes { get; private set; }
        public byte[] Data { get; private set; }

        public TAPBlock(ushort sizeInBytes, byte[] data)
        {
            SizeInBytes = sizeInBytes;
            Data = data[1..^1];
            // we're going to ignore the flag and checksum bytes for now
        }
    }

    public enum TAPType
    {
        Program = 0, 
        NumberArray = 1, 
        CharacterArray = 2, 
        Code = 3
    }
}
