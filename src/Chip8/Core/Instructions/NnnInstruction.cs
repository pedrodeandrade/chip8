namespace Chip8.Core.Instructions;

public sealed record NnnInstruction : Instruction
{
    public NnnInstruction(ReadOnlySpan<byte> instruction) : base(instruction)
    {
        /*
        * DISCLAIMER: I'm just using F to represent a nibble (4 bits number), but it can actually be any hex digit
        * Get 16 bits 0's and OR with MSB with just the first 4 bits set, we'll got something like 0x000F (just least significant nibble set)
        * Then we shift 8 bits right so that the first nibble is placed at the first half of the MSB, we'll have something like 0x0F00
        * Last thing to do is OR with LSB so we can add it as the first byte of the 16 bit value, at the end we'll have 0X0FFF
        */
        Nnn = (ushort)(((0x0000 | (MostSignificantByte & 0X0F)) << 8) | LeastSignificantByte);
    }

    /// <summary>
    /// 12 bits address
    /// </summary>
    public ushort Nnn { get; }
}
