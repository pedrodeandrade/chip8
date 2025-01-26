using Chip8.Core.Instructions;
using Chip8.View;

namespace Chip8.Core.Pipeline;

public class InstructionDecoderExecutor
{
    private const byte MaxBytesPerSprite = 15;
    private const short MemoryMaxAddress = 0xFFF; // 0 to 4095 = 4096 bytes -> 4kb

    public void DecodeAndExec(Instruction instruction, CpuContext context)
    {
        switch (instruction.OpCode)
        {
            case 0x1:
                DecodeAndExecJumpInstruction((NnnInstruction)instruction, context);
                break;
            case 0x6:
                DecodeAndExecSetVRegisterInstruction((XkkInstruction)instruction, context);
                break;
            case 0x7:
                DecodeAndExecAddToVRegisterInstruction((XkkInstruction)instruction, context);
                break;
            case 0xD:
                DecodeAndExecDisplayInstruction(
                    (XynInstruction)instruction,
                    context
                );
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void DecodeAndExecJumpInstruction(NnnInstruction instruction, CpuContext context)
    {
        var jumpAddress = instruction.Nnn;

        // Instruction are two bytes long, and they have to be in addresses multiples of 2 (aligned by 2 bytes);
        if (jumpAddress > MemoryMaxAddress - 1 || jumpAddress % 2 != 0)
            throw new Exception("Invalid address to jump!");

        context.Registers.Pc = jumpAddress;
    }

    private void DecodeAndExecSetVRegisterInstruction(XkkInstruction instruction, CpuContext context)
        => context.Registers.V[instruction.X] = instruction.Kk;

    private void DecodeAndExecAddToVRegisterInstruction(XkkInstruction instruction, CpuContext context)
        => context.Registers.V[instruction.X] += instruction.Kk;

    private void DecodeAndExecDisplayInstruction(XynInstruction instruction, CpuContext context)
    {
        if (instruction.N > MaxBytesPerSprite)
            throw new Exception($"Maximum of {MaxBytesPerSprite} bytes can be displayed at once");

        ReadOnlySpan<byte> sprites = context.Memory
            .AsSpan()
            .Slice(context.Registers.I, instruction.N);

        var x = context.Registers.V[instruction.X];
        var currentY = context.Registers.V[instruction.Y];
        var spriteErasedPixel = false;

        foreach (var spriteByte in sprites)
        {
            Display.SetPixels(x, currentY, spriteByte, out bool pixelErased);
            if (!spriteErasedPixel && pixelErased)
                spriteErasedPixel = true;

            currentY++;
        }

        Display.Render();

        context.Registers.Vf = spriteErasedPixel;
    }
}
