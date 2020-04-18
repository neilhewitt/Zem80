using System.Collections.Generic;

namespace ZexallCSharp
{
    public interface IZexTestSource
    {
        public byte[][] CRCTable { get; }
        public IDictionary<string, TestDescriptor> Tests { get; }
    }
}