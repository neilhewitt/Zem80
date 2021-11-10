using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZXSpectrum.VM
{
    public class TAPFile
    {
        private List<TAPBlock> _blocks = new List<TAPBlock>();

        public string Path { get; private set; }
        public TAPHeader Header { get; private set; }

        public IReadOnlyList<TAPBlock> Blocks => _blocks;

        public byte[] Data => _blocks.SelectMany(x => x.Data).ToArray();

        private void ParseData(byte[] fileData)
        {
            int index = 0;
            while(true)
            {
                ushort blockSize = (ushort)(fileData[index] + (fileData[index + 1] * 256));
                byte flag = fileData[index + 2];
                byte[] blockData = fileData[index..(index + blockSize)];
                if (flag == 0)
                {
                    Header = new TAPHeader(fileData[index..(index + 19)]);
                }
                else
                {
                    TAPBlock block = new TAPBlock(blockSize, blockData);
                    _blocks.Add(block);
                }
                index = index + blockSize - 1;
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
            if (_headerData[2] != 0x00) throw new InvalidDataException("Specified data is not a TAP header.");
            Type = (TAPType)(int)((byte)_headerData[3]);
            Filename = System.Text.ASCIIEncoding.Default.GetString(_headerData[4..13]).TrimEnd();
            LengthInBytes = doubleByteValue(13);
            Parameter1 = doubleByteValue(15);
            Parameter2 = doubleByteValue(16);

            ushort doubleByteValue(int index)
            {
                return (ushort)((byte)_headerData[index] + ((byte)_headerData[index+1] * 256));
            }
        }


        public TAPHeader(byte[] headerData)
        {
            _headerData = headerData;
            ParseHeader();
        }
    }

    public class TAPBlock
    {
        public ushort SizeInBytes { get; private set; }
        public byte[] Data { get; private set; }

        public TAPBlock(ushort sizeInBytes, byte[] data)
        {
            SizeInBytes = sizeInBytes;
            Data = data[2..^1];
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
