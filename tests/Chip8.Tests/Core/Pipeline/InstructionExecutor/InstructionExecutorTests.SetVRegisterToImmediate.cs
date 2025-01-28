using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_SetVRegisterToImmediate
{
    public static class SetVRegisterToImmediateDataSource
    {
        public static IEnumerable<Func<(byte, byte, byte[])>> TestData()
        {
            const byte immediate = 0xFF;

            foreach (var i in Enumerable.Range(0x0, 0x10))
            {
                yield return () => ((byte)i, immediate, [(byte)(0x60 + (byte)i), immediate]);
            }
        }
    }

    [Test]
    [MethodDataSource(typeof(SetVRegisterToImmediateDataSource), nameof(SetVRegisterToImmediateDataSource.TestData))]
    public async Task Execute_ShouldSetVRegisterToImmediate_WhenSetVRegisterToImmediateInstructionIsExecutedSuccessfully((byte , byte, byte[]) testData)
    {
        (var indexRegisterV, var value, var instructionBytes) = testData;
        var cpuContext = new CpuContext();
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XkkInstruction(instructionBytes), cpuContext);

        await Assert.That(cpuContext.Registers.V[indexRegisterV]).IsEqualTo(value);
    }
}
