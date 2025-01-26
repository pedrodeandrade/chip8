namespace Chip8.Core.Instructions;

public sealed record XkkInstruction : Instruction
{
    public XkkInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        X = (byte)(MostSignificantByte & 0x0F);
        Kk = LeastSignificantByte;
    }

    /// <summary>
    /// Index of register V
    /// </summary>
    public byte X { get; }

    /// <summary>
    /// 8-bits immediate number
    /// </summary>
    public byte Kk { get; }
}
