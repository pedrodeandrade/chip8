namespace Chip8.Core.Instructions;

public sealed record XyVariantInstruction : Instruction
{
    public XyVariantInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        X = (byte)(MostSignificatByte & 0X01);
        Y = (byte)(LeastSignificantByte & 0X10);
        Variant = (byte)(LeastSignificantByte & 0X01);
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
