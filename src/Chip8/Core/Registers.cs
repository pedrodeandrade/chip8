namespace Chip8.Core;

public sealed class Registers
{
    public byte V0 { get; set; }

    public byte V1 { get; set; }

    public byte V2 { get; set; }

    public byte V3 { get; set; }

    public byte V4 { get; set; }

    public byte V5 { get; set; }

    public byte V6 { get; set; }

    public byte V7 { get; set; }

    public byte V8 { get; set; }

    public byte V9 { get; set; }

    public byte Va { get; set; }

    public byte Vb { get; set; }

    public byte Vc { get; set; }

    public byte Ve { get; set; }

    public byte Vd { get; set; }

    public byte Vf { get; set; }

    public byte Sp { get; set; }

    public short I { get; set; }

    public short Pc { get; set; }
}
