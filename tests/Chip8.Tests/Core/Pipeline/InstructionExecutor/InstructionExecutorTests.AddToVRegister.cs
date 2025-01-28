using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_AddToVRegister
{
    public static class AddToVRegisterDataSource
    {
        private const byte OpCode = 0x70;

        public static IEnumerable<Func<(byte, byte, byte, byte[])>> AddValuesWithoutOverflowTestData()
        {
            const byte valueToAdd = 0x04;
            const byte initialRegisterValue = 0x03;

            foreach (var i in Enumerable.Range(0x0, 0x10))
            {
                yield return () => ((byte)i, initialRegisterValue, valueToAdd, [(byte)(OpCode + (byte)i), valueToAdd]);
            }
        }

        public static IEnumerable<Func<(byte, byte, byte, byte[])>> AddValuesWithOverflowTestData()
        {
            const byte valueToAdd = 0xAE;
            const byte initialRegisterValue = 0xFF;

            foreach (var i in Enumerable.Range(0x0, 0x10))
            {
                yield return () => ((byte)i, initialRegisterValue, valueToAdd, [(byte)(OpCode + (byte)i), valueToAdd]);
            }
        }
    }

    [Test]
    [MethodDataSource(typeof(AddToVRegisterDataSource), nameof(AddToVRegisterDataSource.AddValuesWithoutOverflowTestData))]
    [MethodDataSource(typeof(AddToVRegisterDataSource), nameof(AddToVRegisterDataSource.AddValuesWithOverflowTestData))]
    public async Task Execute_ShouldAddValueToVRegister_WhenTheValueAddedToVRegisterInstructionIsExecuted((byte, byte, byte, byte[]) testData)
    {
        (var indexRegisterV, var initialRegisterValue, var valueToAdd, var instructionBytes) = testData;
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[indexRegisterV] = initialRegisterValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XkkInstruction(instructionBytes), cpuContext);

        await Assert.That(cpuContext.Registers.V[indexRegisterV]).IsEqualTo((byte)(initialRegisterValue + valueToAdd));
    }
}
