using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionDecoderExecutorImpl = Chip8.Core.Pipeline.InstructionDecoderExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionDecoderExecutor;

public class InstructionDecoderExecutorTests_SetIndexRegister
{
    [Test]
    public async Task DecodeAndExec_ShouldSetValueToIndexRegister_WhenSetIndexRegisterInstructionIsExecutedSucceessfully()
    {
        var cpuContext = new CpuContext();
        var instruction = new NnnInstruction([0xAF, 0xFF]);
        var value = (ushort)0xFFF;
        var sut = new InstructionDecoderExecutorImpl();

        sut.DecodeAndExec(instruction, cpuContext);

        await Assert.That(cpuContext.Registers.I).IsEqualTo(value);
    }
}
