namespace Chip8.Core.Pipeline;

public static class InstructionFetcher
{
    public static ushort Fetch(CpuContext context)
    {
        byte firstHalfInstruction = context.Memory[context.Registers.Pc];
        byte secondHalfInstruction = context.Memory[context.Registers.Pc + 1];

        context.Registers.Pc += 2;

        // The idea here is to get the first and second bytes from the instruction and store then side to side in a 16-bit variable to form the instruction
        return (ushort)(((0x0000 | firstHalfInstruction) << 8) | secondHalfInstruction);
    }
}
