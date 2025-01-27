// See https://aka.ms/new-console-template for more information

using Chip8.Core;

public static class Program
{
    public async static Task Main(string[] args)
    {
        var program = await File.ReadAllBytesAsync(args[0]);

        Cpu cpu = new(new());
        cpu.LoadProgram(program);
        cpu.Start();
    }
}
