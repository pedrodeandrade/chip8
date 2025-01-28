using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_SetIndexRegister
{
    [Test]
    public async Task Execute_ShouldSetValueToIndexRegister_WhenSetIndexRegisterInstructionIsExecutedSucceessfully()
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction([0xAF, 0xFF]);
        var value = (ushort)0xFFF;
        var sut = new InstructionExecutorImpl();

        sut.Execute(instruction, cpuContext);

        await Assert.That(cpuContext.Registers.I).IsEqualTo(value);
    }
}
