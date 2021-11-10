using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZXSpectrum.VM
{
    public static class TAPLoader
    {
        public static byte[] LoadToBytes(string path)
        {
            if (File.Exists(path))
            {
                byte[] data = File.ReadAllBytes(path);
                TAPFile tape = new TAPFile(data);
                return tape.Data;
            }
            else
            {
                throw new FileLoadException("Specified file does not exist.");
            }
        }
    }
}
