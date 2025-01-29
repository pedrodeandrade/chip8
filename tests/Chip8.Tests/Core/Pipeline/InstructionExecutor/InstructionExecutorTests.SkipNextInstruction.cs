using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_SkipNextInstruction
{
    [Test]
    public async Task Execute_ShouldIncrementPcRegisterBy2_WhenSkipNextInstructionInstructionIsExecutedAndVxAndVyAreDifferent()
    {
        var originalPcValue = (ushort)0x0111;
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[0] = 0x01;
        cpuContext.Registers.V[1] = 0x02;
        cpuContext.Registers.Pc = originalPcValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x90, 0x10]), cpuContext);

        await Assert.That(cpuContext.Registers.Pc).IsEqualTo((ushort)(originalPcValue + 2));
    }

    [Test]
    public async Task Execute_ShouldIncrementPcRegisterByTwo_WhenSkipNextInstructionInstructionIsExecutedAndVxAndVyAreEqual()
    {
        var originalPcValue = (ushort)0x0111;
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[0] = 0x01;
        cpuContext.Registers.V[1] = 0x01;
        cpuContext.Registers.Pc = originalPcValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x90, 0x10]), cpuContext);

        await Assert.That(cpuContext.Registers.Pc).IsEqualTo(originalPcValue);
    }
}
