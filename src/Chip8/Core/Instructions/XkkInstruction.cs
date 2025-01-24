namespace Chip8.Core.Instructions;

public sealed record XkkInstruction : Instruction
{
    public XkkInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        X = (byte)(MostSignificatByte & 0X01);
        Kk = MostSignificatByte;
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
