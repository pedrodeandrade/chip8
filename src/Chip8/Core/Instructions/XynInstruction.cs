namespace Chip8.Core.Instructions;

public sealed record XynInstruction : Instruction
{
    public XynInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        X = (byte)(MostSignificatByte & 0X01);
        Y = (byte)(LeastSignificantByte & 0X10);
        N = (byte)(LeastSignificantByte & 0X01);
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
    /// Number
    /// </summary>
    public byte N { get; }
};
