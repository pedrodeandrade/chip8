namespace Chip8.Core.Instructions;

public record Instruction
{
    public Instruction(ReadOnlySpan<byte> instruction)
    {
        if (instruction.Length != 2)
            throw new Exception("A instruction can only 2 bytes long!");

        MostSignificantByte = instruction[0];
        LeastSignificantByte = instruction[1];
        OpCode = (byte)(MostSignificantByte >> 4);
    }

    public byte OpCode { get; }

    protected byte MostSignificantByte { get; }

    protected byte LeastSignificantByte { get; }
}
