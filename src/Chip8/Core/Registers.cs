namespace Chip8.Core;

public sealed class Registers
{
    public byte[] V { get; } = new byte[0x10];

    public byte Sp { get; set; }

    public ushort I { get; set; }

    public ushort Pc { get; set; }

    public bool Vf { get; set; }
}
