namespace Zem80.Core.CPU
{
    public interface IIO
    {
        bool A0 { get; }
        bool A1 { get; }
        bool A10 { get; }
        bool A11 { get; }
        bool A12 { get; }
        bool A13 { get; }
        bool A14 { get; }
        bool A15 { get; }
        bool A2 { get; }
        bool A3 { get; }
        bool A4 { get; }
        bool A5 { get; }
        bool A6 { get; }
        bool A7 { get; }
        bool A8 { get; }
        bool A9 { get; }
        ushort ADDRESS_BUS { get; }
        bool D0 { get; }
        bool D1 { get; }
        bool D2 { get; }
        bool D3 { get; }
        bool D4 { get; }
        bool D5 { get; }
        bool D6 { get; }
        bool D7 { get; }
        byte DATA_BUS { get; }
        bool INT { get; }
        bool IORQ { get; }
        bool M1 { get; }
        bool MREQ { get; }
        bool NMI { get; }
        bool RD { get; }
        bool RESET { get; }
        bool WAIT { get; }
        bool WR { get; }

        void AddMemoryData(byte data);
        void AddOpcodeFetchData(byte data);
        void AddPortReadData(byte data);
        void Clear();
        void EndInterruptState();
        void EndMemoryReadState();
        void EndMemoryWriteState();
        void EndNMIState();
        void EndOpcodeFetchState();
        void EndPortReadState();
        void EndPortWriteState();
        void EndWaitState();
        void SetAddressBusValue(ushort value);
        void SetDataBusDefault(byte defaultValue);
        void SetDataBusValue(byte value);
        void ResetDataBusValue();
        void SetInterruptState();
        void SetMemoryReadState(ushort address);
        void SetMemoryWriteState(ushort address, byte data);
        void SetNMIState();
        void SetOpcodeFetchState(ushort address);
        void SetPortReadState(ushort portAddress);
        void SetPortWriteState(ushort portAddress, byte data);
        void SetResetState();
        void SetWaitState();
    }
}