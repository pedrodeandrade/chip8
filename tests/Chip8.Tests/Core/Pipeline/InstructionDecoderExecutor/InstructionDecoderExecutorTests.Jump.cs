using Chip8.Core;
using Chip8.Core.Instructions;

namespace Chip8.Tests.Core.Pipeline.InstructionDecoderExecutor;

public class InstructionDecoderExecutorTests_Jump
{
    [Test]
    public void DecodeAndExec_ShouldThrowException_WhenJumpInstructionIsExecutedAndJumpAddressExceedsMemoryBounds()
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction([0x1F, 0xFF]);

        Assert.Throws(() => Chip8.Core.Pipeline.InstructionDecoderExecutor.DecodeAndExec(instruction, cpuContext));
    }

    [Test]
    public void DecodeAndExec_ShouldThrowException_WhenJumpInstructionIsExecutedAndJumpAddressIsNotTwoBytesAligned()
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction([0x11, 0x4B]);

        Assert.Throws(() => Chip8.Core.Pipeline.InstructionDecoderExecutor.DecodeAndExec(instruction, cpuContext));
    }

    [Test]
    [Arguments((ushort)0xFFE, new byte[] { 0x1F, 0xFE })]
    [Arguments((ushort)0x112, new byte[] { 0x11, 0x12 })]
    [Arguments((ushort)0x14A, new byte[] { 0x11, 0x4A })]
    public async Task DecodeAndExec_ShouldSetPcToAddress_WhenJumpInstructionIsExecutedWithValidJumpAddress(ushort jumpAddress, byte[] instructionBytes)
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction(instructionBytes);

        Chip8.Core.Pipeline.InstructionDecoderExecutor.DecodeAndExec(instruction, cpuContext);

        await Assert.That(cpuContext.Registers.Pc).IsEqualTo(jumpAddress);
    }
}
