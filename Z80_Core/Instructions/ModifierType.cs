namespace Z80.Core
{
    public enum ModifierType
    {
        None,
        ByteRegister,
        FromAddress,
        ByteRegisterPair,
        ByteRegisterFromAddress,
        ByteRegisterFromImmediate,
        AddressFromByteRegister,
        WordRegister,
        WordRegisterPair,
        WordRegisterFromAddress,
        WordRegisterFromImmediate,
        IORegister,
        BitAtAddress,
        BitOfRegister,
        IndexRegister,
        IndexRegisterHalf
    }
}
