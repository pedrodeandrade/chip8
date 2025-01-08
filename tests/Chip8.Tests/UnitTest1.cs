namespace Chip8.Tests;

public class UnitTest1
{
    [Test]
    public async Task Test1()
    {
        var result = 3;
        await Assert.That(result).IsEqualTo(3);
    }
}
