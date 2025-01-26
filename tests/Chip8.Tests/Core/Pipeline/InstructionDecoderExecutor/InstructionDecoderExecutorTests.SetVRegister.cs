using Chip8.Core;
using Chip8.Core.Instructions;
using InstructionDecoderExecutorImpl = Chip8.Core.Pipeline.InstructionDecoderExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionDecoderExecutor;

public class InstructionDecoderExecutorTests_SetVRegister
{
    public static class SetVRegisterDataSource
    {
        public static IEnumerable<Func<(byte, byte, byte[])>> TestData()
        {
            foreach (var i in Enumerable.Range(0x0, 0x10))
            {
                yield return () => ((byte)i, 0xFF, [(byte)(0x60 + (byte)i), 0xFF]);
            }
        }
    }

    [Test]
    [MethodDataSource(typeof(SetVRegisterDataSource), nameof(SetVRegisterDataSource.TestData))]
    public async Task DecodeAndExec_ShouldSetVRegisterToValue_WhenSetVRegisterInstructionIsExecutedSuccessfully((byte , byte, byte[]) testData)
    {
        (var indexRegisterV, var value, var instructionBytes) = testData;
        var cpuContext = new CpuContext();
        var sut = new InstructionDecoderExecutorImpl();

        sut.DecodeAndExec(new XkkInstruction(instructionBytes), cpuContext);

        await Assert.That(cpuContext.Registers.V[indexRegisterV]).IsEqualTo(value);
    }
}
