using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_Random
{
    [Test]
    public async Task Execute_SetRegisterToRandomValue_WhenRandomInstructionIsExecutedWithValidValues()
    {
        var initialRegisterValue = (byte)1;
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[0xE] = initialRegisterValue;
        var instruction = new XkkInstruction([0xCE, 0xEF]);
        var sut = new InstructionExecutorImpl();

        sut.Execute(instruction, cpuContext);

        await Assert.That(cpuContext.Registers.V[0xE]).IsNotEqualTo(initialRegisterValue);
    }
}
