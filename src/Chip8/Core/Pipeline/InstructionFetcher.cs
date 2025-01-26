using Chip8.Core.Instructions;

namespace Chip8.Core.Pipeline;

public static class InstructionFetcher
{
    public static Instruction Fetch(CpuContext context)
    {
        var instruction = (ReadOnlySpan<byte>)context.Memory
            .AsSpan()
            .Slice(context.Registers.Pc, 2);
        var opcode = (byte)(instruction[0] >> 4);

        context.Registers.Pc += 2;

        return opcode switch
        {
            0x01 or 0x02 or 0x0A or 0x0B => new NnnInstruction(instruction),
            0x03 or 0X04 or 0x06 or 0x07 or 0x0C => new XkkInstruction(instruction),
            0x05 or 0x08 or 0x09 => new XyVariantInstruction(instruction),
            0x0D => new XynInstruction(instruction),
            0xE or 0xF => new XVariantInstruction(instruction),
            _ => new Instruction(instruction)
        };
    }
}
