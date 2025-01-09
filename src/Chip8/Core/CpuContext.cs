namespace Chip8.Core;

public sealed class CpuContext
{
    public byte[] Memory { get; } = new byte[4096];

    public Registers Registers { get; } = new();

    public Stack<ushort> Stack { get; } = new(16);
}
