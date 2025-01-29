using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_Jump
{
    [Test]
    public void Execute_ShouldThrowException_WhenJumpInstructionIsExecutedAndJumpAddressExceedsMemoryBounds()
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction([0x1F, 0xFF]);
        var sut = new InstructionExecutorImpl();

        Assert.Throws(() => sut.Execute(instruction, cpuContext));
    }

    [Test]
    [Arguments((ushort)0xFFE, new byte[] { 0x1F, 0xFE })]
    [Arguments((ushort)0x112, new byte[] { 0x11, 0x12 })]
    [Arguments((ushort)0x14A, new byte[] { 0x11, 0x4A })]
    public async Task Execute_ShouldSetPcToAddress_WhenJumpInstructionIsExecutedWithValidJumpAddress(ushort jumpAddress, byte[] instructionBytes)
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction(instructionBytes);
        var sut = new InstructionExecutorImpl();

        sut.Execute(instruction, cpuContext);

        await Assert.That(cpuContext.Registers.Pc).IsEqualTo(jumpAddress);
    }
}
