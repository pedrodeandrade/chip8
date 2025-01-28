using Chip8.Core.Instructions;
using Chip8.Core.Pipeline;

namespace Chip8.Core;

public sealed class Cpu
{
    private const ushort InitialAddress = 0x200;

    private readonly CpuContext _context;
    private readonly InstructionFetcher _instructionFetcher = new();
    private readonly InstructionExecutor _instructionExecutor = new();

    public Cpu(CpuContext context)
    {
        _context = context;
        InitFonts();
        InitMemory();
        InitRegisters();
    }

    public void Start()
    {
        while (true)
        {
            ExecuteInstruction(FetchAndDecodeInstruction());
        }
    }

    public void LoadProgram(ReadOnlySpan<byte> program)
    {
        var initialAddress = InitialAddress;

        foreach (byte programByte in program)
        {
            _context.Memory[initialAddress] = programByte;
            initialAddress++;
        }
    }

    private Instruction FetchAndDecodeInstruction()
        => _instructionFetcher.FetchAndDecode(_context);

    private void ExecuteInstruction(Instruction instruction)
        => _instructionExecutor.Execute(instruction, _context);

    private void InitFonts()
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
    private void InitMemory()
    {
        _context.Memory[InitialAddress] = 0x02;
        _context.Memory[InitialAddress + 1] = 0x00;
    }

    private void InitRegisters()
    {
        _context.Registers.Pc = InitialAddress;
        _context.Registers.Vf = false;
    }
}
