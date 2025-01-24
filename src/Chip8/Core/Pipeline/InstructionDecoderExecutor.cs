using Chip8.View;

namespace Chip8.Core.Pipeline;

public static class InstructionDecoderExecutor
{
    private const byte MaxBytesPerSprite = 15;

    public static void DecodeAndExec(ushort instruction, CpuContext context)
    {
        ReadOnlySpan<byte> instructionBytes = stackalloc byte[]
        {
            (byte)(instruction >> 8),
            (byte)instruction
        };

        byte opcode = GetMostSignificantNibble(instructionBytes[0]);
        switch (opcode)
        {
            case 0xD:
                DecodeAndExecDisplayInstruction(
                    registerX: GetLeastSignificantNibble(instructionBytes[0]),
                    registerY: GetMostSignificantNibble(instructionBytes[1]),
                    n: GetLeastSignificantNibble(instructionBytes[1]),
                    context: context
                );
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private static void DecodeAndExecDisplayInstruction(byte registerX, byte registerY, byte n, CpuContext context)
    {
        if (n > MaxBytesPerSprite)
            throw new Exception($"Maximum of {MaxBytesPerSprite} bytes can be displayed at once");

        ReadOnlySpan<byte> sprites = context.Memory
            .AsSpan()
            .Slice(context.Registers.I, n);

        var x = context.Registers.V[registerX];
        var currentY = context.Registers.V[registerY];
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

    private static byte GetLeastSignificantNibble(byte data)
        => (byte)(0X0F & data);

    private static byte GetMostSignificantNibble(byte data)
        => (byte)(data >> 4);
}
