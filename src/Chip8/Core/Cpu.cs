using Chip8.Core.Pipeline;

namespace Chip8.Core;

public static class Cpu
{
    private const ushort InitialAddress = 0x200;

    private static readonly CpuContext _context = new();

    static Cpu()
    {
        InitFonts();
        InitMemory();
        InitRegisters();
    }

    public static void Start()
    {
        while (true)
        {
            DecodeAndExecuteInstruction(FetchInstruction());
        }
    }

    public static void LoadProgram(ReadOnlySpan<byte> program)
    {
        var initialAddress = InitialAddress;

        foreach (byte programByte in program)
        {
            _context.Memory[initialAddress] = programByte;
            initialAddress++;
        }
    }

    private static ushort FetchInstruction()
        => InstructionFetcher.Fetch(_context);

    private static void DecodeAndExecuteInstruction(ushort instruction)
        => InstructionDecoderExecutor.DecodeAndExec(instruction, _context);

    private static void InitFonts()
    {
        Span<byte> fonts = stackalloc byte[]
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        var fontAddress = 0x050;

        foreach (var fontByte in fonts)
        {
            _context.Memory[fontAddress] = fontByte;
            fontAddress++;
        }
    }

    /// <summary>
    /// Initialize the cpu in a loop that always redirect to the initial instruction
    /// </summary>
    private static void InitMemory()
    {
        _context.Memory[InitialAddress] = 0x02;
        _context.Memory[InitialAddress + 1] = 0x00;
    }

    private static void InitRegisters()
    {
        _context.Registers.Pc = InitialAddress;
        _context.Registers.Vf = false;
    }
}
