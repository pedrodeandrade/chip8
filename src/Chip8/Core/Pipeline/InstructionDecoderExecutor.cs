using Chip8.Core.Instructions;
using Chip8.View;

namespace Chip8.Core.Pipeline;

public static class InstructionDecoderExecutor
{
    private const byte MaxBytesPerSprite = 15;

    public static void DecodeAndExec(Instruction instruction, CpuContext context)
    {
        switch (instruction.OpCode)
        {
            case 0xD:
                DecodeAndExecDisplayInstruction(
                    (XynInstruction)instruction,
                    context: context
                );
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private static void DecodeAndExecDisplayInstruction(XynInstruction instruction, CpuContext context)
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
