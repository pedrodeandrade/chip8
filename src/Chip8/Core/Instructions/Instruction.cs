namespace Chip8.Core.Instructions;

public abstract record Instruction
{
    protected Instruction(ReadOnlySpan<byte> instruction)
    {
        if (instruction.Length != 2)
            throw new Exception("A instruction can only 2 bytes long!");

        MostSignificatByte = instruction[0];
        LeastSignificantByte = instruction[1];
        OpCode = (byte)(MostSignificatByte >> 4);
    }

    public byte OpCode { get; }

    public byte MostSignificatByte { get; }

    public byte LeastSignificantByte { get; }
}
