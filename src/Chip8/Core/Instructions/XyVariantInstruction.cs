namespace Chip8.Core.Instructions;

public sealed record XyVariantInstruction : Instruction
{
    public XyVariantInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        X = (byte)(MostSignificantByte & 0x0F);
        Y = (byte)((LeastSignificantByte & 0xF0) >> 4);
        Variant = (byte)(LeastSignificantByte & 0x0F);
    }

    /// <summary>
    /// Index x of register V
    /// </summary>
    public byte X { get; }

    /// <summary>
    /// Index y of register V
    /// </summary>
    public byte Y { get; }

    /// <summary>
    /// Instruction variant number
    /// </summary>
    public byte Variant { get; }
}
