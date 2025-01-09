using Chip8.Core;
using Chip8.Core.Pipeline;

namespace Chip8.Tests.Core.Pipeline;

public class InstructionFetcherTests
{
    [Test]
    public async Task InstructionFetcher_ShouldReturnsCorrectInstruction_WhenInstructionIsRequested()
    {
        var loadedInstruction = (ushort)0x1000;

        var context = new CpuContext();
        context.Registers.Pc = 0x200;
        context.Memory[0x200] = (byte)(loadedInstruction >> 8);
        context.Memory[0x201] = (byte)loadedInstruction;

        ushort fetchedInstruction = InstructionFetcher.Fetch(context);

        await Assert.That(loadedInstruction).IsEqualTo(fetchedInstruction);
    }
}
