using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_JumpWithOffset
{
    [Test]
    public async Task Execute_ShouldSetPcToVxPlusOffsetLocation_WhenJumpWithOffsetInstructionIsExecutedSuccessfully()
    {
        var vxIndex = 0xF; // x
        var offset = (byte)0x10; // kk
        var xkk = 0xF10;
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = offset;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XkkInstruction([0xBF, 0x10]), cpuContext);

        await Assert.That(cpuContext.Registers.Pc).IsEqualTo((ushort)(xkk + cpuContext.Registers.V[vxIndex]));
    }

    [Test]
    public Task Execute_ShouldThrowException_WhenJumpWithOffsetInstructionIsExecutedAndJumpAddressIsLongerThanTwelveBits()
    {
        var vxIndex = 0xF; // x
        var offset = (byte)0xFF; // kk
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = offset;
        var sut = new InstructionExecutorImpl();

        Assert.Throws(() => sut.Execute(new XkkInstruction([0xBF, 0xFF]), cpuContext));

        return Task.CompletedTask;
    }
}
