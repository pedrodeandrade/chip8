using Chip8.Core;
using Chip8.Core.Instructions;
using Chip8.Core.Pipeline;

namespace Chip8.Tests.Core.Pipeline;

public class InstructionFetcherTests
{
     [Test]
     [Arguments((ushort)0x1000, typeof(NnnInstruction))]
     [Arguments((ushort)0x3000, typeof(XkkInstruction))]
     [Arguments((ushort)0x5000, typeof(XyVariantInstruction))]
     [Arguments((ushort)0xD000, typeof(XynInstruction))]
     [Arguments((ushort)0xF000, typeof(XVariantInstruction))]
     public async Task InstructionFetcher_ShouldReturnsCorrectInstruction_WhenInstructionIsRequested(ushort instruction, Type instructionType) {
         var context = new CpuContext();
         context.Registers.Pc = 0x200;
         context.Memory[0x200] = (byte)(instruction >> 8);
         context.Memory[0x201] = (byte)instruction;

         Instruction fetchedInstruction = new InstructionFetcher().FetchAndDecode(context);

         await Assert.That(fetchedInstruction).IsTypeOf(instructionType);
     }
}
