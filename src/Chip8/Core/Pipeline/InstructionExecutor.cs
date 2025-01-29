using Chip8.Core.Instructions;
using Chip8.View;

namespace Chip8.Core.Pipeline;

public sealed class InstructionExecutor
{
    private const byte MaxBytesPerSprite = 15;
    private const short MemoryMaxAddress = 0xFFF; // 0 to 4095 = 4096 bytes -> 4kb

    public void Execute(Instruction instruction, CpuContext context)
    {
        switch (instruction.OpCode)
        {
            case 0x0:
                ExecuteClearScreenInstruction();
                break;
            case 0x1:
                ExecuteJumpInstruction((NnnInstruction)instruction, context);
                break;
            case 0x6:
                ExecuteSetVRegisterToImmediateInstruction((XkkInstruction)instruction, context);
                break;
            case 0x7:
                ExecuteAddToVRegisterInstruction((XkkInstruction)instruction, context);
                break;
            case 0x8:
                ExecuteLogicalArithmeticInstruction((XyVariantInstruction)instruction, context);
                break;
            case 0xA:
                ExecuteSetIndexRegisterInstruction((NnnInstruction)instruction, context);
                break;
            case 0xD:
                ExecuteDisplayInstruction((XynInstruction)instruction, context);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void ExecuteClearScreenInstruction()
        => Display.Clear();

    private void ExecuteJumpInstruction(NnnInstruction instruction, CpuContext context)
    {
        var jumpAddress = instruction.Nnn;

        // Instruction are two bytes long, and they have to be in addresses multiples of 2 (aligned by 2 bytes);
        if (jumpAddress > MemoryMaxAddress - 1 || jumpAddress % 2 != 0)
            throw new Exception("Invalid address to jump!");

        context.Registers.Pc = jumpAddress;
    }

    private void ExecuteSetVRegisterToImmediateInstruction(XkkInstruction instruction, CpuContext context)
        => context.Registers.V[instruction.X] = instruction.Kk;

    private void ExecuteAddToVRegisterInstruction(XkkInstruction instruction, CpuContext context)
        => context.Registers.V[instruction.X] += instruction.Kk;

    private void ExecuteLogicalArithmeticInstruction(XyVariantInstruction instruction, CpuContext context)
    {
        switch (instruction.Variant)
        {
            // Set Vx to Vy
            case 0:
                context.Registers.V[instruction.X] = context.Registers.V[instruction.Y];
                break;
            // Binary Or Vx and Vy
            case 1:
                context.Registers.V[instruction.X] |= context.Registers.V[instruction.Y];
                break;
            // Binary And Vx and Vy
            case 2:
                context.Registers.V[instruction.X] &= context.Registers.V[instruction.Y];
                break;
            // Xor between Vx and Vy
            case 3:
                context.Registers.V[instruction.X] ^= context.Registers.V[instruction.Y];
                break;
            // Add Vx and Vy
            case 4:
                byte result = 0;

                // Need to check if overflow happens, if it does, then set Vf to 1, else don't
                try
                {
                    result = checked((byte)(context.Registers.V[instruction.X] + context.Registers.V[instruction.Y]));
                }
                catch (OverflowException)
                {
                    result = (byte)(context.Registers.V[instruction.X] + context.Registers.V[instruction.Y]);
                    context.Registers.Vf = true;
                }
                finally
                {
                    context.Registers.V[instruction.X] = result;
                }

                break;
            // Subtract Vx and Vy
            case 5:
                byte vxValueVariant5 = context.Registers.V[instruction.X];
                byte vyValueVariant5 = context.Registers.V[instruction.Y];

                context.Registers.V[instruction.X] = (byte)(vxValueVariant5 - vyValueVariant5);
                context.Registers.Vf = vxValueVariant5 > vyValueVariant5;

                break;
            // shift right Vx (CHIP-8/SUPER-CHIP version, aka modern version)
            case 6:
                byte vxValueVariant6 = context.Registers.V[instruction.X];

                context.Registers.V[instruction.X] = (byte)(vxValueVariant6 >> 1);
                context.Registers.Vf = Convert.ToBoolean(vxValueVariant6 & 0x01);

                break;
            // Subtract Vy and Vx
            case 7:
                byte vxValueVariant7 = context.Registers.V[instruction.X];
                byte vyValueVariant7 = context.Registers.V[instruction.Y];

                context.Registers.V[instruction.Y] = (byte)(vyValueVariant7 - vxValueVariant7);
                context.Registers.Vf = vyValueVariant7 > vxValueVariant7;

                break;
            // shift left Vx (CHIP-8/SUPER-CHIP version, aka modern version)
            case 0xE:
                byte vxValueVariantE = context.Registers.V[instruction.X];

                context.Registers.V[instruction.X] = (byte)(vxValueVariantE << 1);
                context.Registers.Vf = Convert.ToBoolean(vxValueVariantE & 0x01);

                break;
        }
    }

    private void ExecuteSetIndexRegisterInstruction(NnnInstruction instruction, CpuContext context)
        => context.Registers.I = instruction.Nnn;

    private void ExecuteDisplayInstruction(XynInstruction instruction, CpuContext context)
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
