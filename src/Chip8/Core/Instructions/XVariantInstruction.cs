namespace Chip8.Core.Instructions;

public sealed record XVariantInstruction : Instruction
{
    public XVariantInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        X = (byte)(MostSignificantByte & 0x0F);
        Variant = LeastSignificantByte;
    }

    /// <summary>
    /// Index of register V
    /// </summary>
    public byte X { get; }

    /// <summary>
    /// Instruction variant number
    /// </summary>
    public byte Variant { get; }
}
