using Chip8.Core;
using Chip8.Core.Instructions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace Chip8.Tests.Core.Pipeline.InstructionExecutor;

using InstructionExecutorImpl = Chip8.Core.Pipeline.InstructionExecutor;

public class InstructionExecutorTests_LogicalArithmeticInstruction
{
    [Test]
    public async Task Execute_ShouldSetVxRegisterToVyRegister_WhenSetVxRegisterToVyInstructionIsSuccessfullyExecuted()
    {
        var cpuContext = new CpuContext();
        var vxIndex = 0;
        var vyIndex = 1;
        cpuContext.Registers.V[vxIndex] = 0x01;
        cpuContext.Registers.V[vyIndex] = 0xFF;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x80, 0x10]), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo(cpuContext.Registers.V[vyIndex]);
    }

    [Test]
    public async Task
        Execute_ShouldSetVxRegisterToBinaryOrWithVyRegister_WhenBinaryOrVariantInstructionIsSuccessfullyExecuted()
    {
        var cpuContext = new CpuContext();
        var vxIndex = 0;
        var vyIndex = 1;
        byte vxValue = 0x10;
        byte vyValue = 0x01;
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.V[vyIndex] = vyValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x80, 0x11]), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue | vyValue));
    }

    [Test]
    public async Task
        Execute_ShouldSetVxRegisterToBinaryAndWithVyRegister_WhenBinaryAndVariantInstructionIsSuccessfullyExecuted()
    {
        var cpuContext = new CpuContext();
        var vxIndex = 0;
        var vyIndex = 1;
        byte vxValue = 0x11;
        byte vyValue = 0x01;
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.V[vyIndex] = vyValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x80, 0x12]), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue & vyValue));
    }

    [Test]
    public async Task Execute_ShouldSetVxRegisterToXorWithVyRegister_WhenXorVariantInstructionIsSuccessfullyExecuted()
    {
        var cpuContext = new CpuContext();
        var vxIndex = 0;
        var vyIndex = 1;
        byte vxValue = 0x10;
        byte vyValue = 0x01;
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.V[vyIndex] = vyValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x80, 0x13]), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue ^ vyValue));
    }

    [Test]
    [Arguments((byte)0, (byte)0x10, (byte)1, (byte)0x01, false, new byte[] { 0x80, 0x14 })]
    [Arguments((byte)0, (byte)0xFF, (byte)1, (byte)0xAF, true, new byte[] { 0x80, 0x14 })]
    public async Task Execute_ShouldAddVxRegisterWithVyRegister_WhenAddVariantInstructionIsSuccessfullyExecuted(
        byte vxIndex,
        byte vxValue,
        byte vyIndex,
        byte vyValue,
        bool overflow,
        byte[] instructionBytes
    )
    {
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.V[vyIndex] = vyValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction(instructionBytes), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue + vyValue));
        await Assert.That(cpuContext.Registers.Vf).IsEqualTo(overflow);
    }

    [Test]
    [Arguments((byte)0, (byte)0x10, (byte)1, (byte)0x01, true, new byte[] { 0x80, 0x15 })]
    [Arguments((byte)0, (byte)0x01, (byte)1, (byte)0x10, false, new byte[] { 0x80, 0x15 })]
    [Arguments((byte)0, (byte)0x01, (byte)1, (byte)0x01, false, new byte[] { 0x80, 0x15 })] // Vf doesn't change when vxValue and vyValue are the same
    public async Task Execute_ShouldSubtractVyRegisterFromVxRegister_WhenSubtractVyFromVxVariantInstructionIsSuccessfullyExecuted(
        byte vxIndex,
        byte vxValue,
        byte vyIndex,
        byte vyValue,
        bool carry,
        byte[] instructionBytes
    )
    {
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.V[vyIndex] = vyValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction(instructionBytes), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue - vyValue));
        await Assert.That(cpuContext.Registers.Vf).IsEqualTo(carry);
    }

    [Test]
    [Arguments((byte)0, (byte)0x01, (byte)1, (byte)0x10, true, new byte[] { 0x80, 0x17 })]
    [Arguments((byte)0, (byte)0x10, (byte)1, (byte)0x01, false, new byte[] { 0x80, 0x17 })]
    [Arguments((byte)0, (byte)0x01, (byte)1, (byte)0x01, false, new byte[] { 0x80, 0x17 })] // Vf doesn't change when vxValue and vyValue are the same
    public async Task Execute_ShouldSubtractVxRegisterFromVyRegister_WhenSubtractVxFromVyVariantInstructionIsSuccessfullyExecuted(
        byte vxIndex,
        byte vxValue,
        byte vyIndex,
        byte vyValue,
        bool carry,
        byte[] instructionBytes
    )
    {
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.V[vyIndex] = vyValue;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction(instructionBytes), cpuContext);

        await Assert.That(cpuContext.Registers.V[vyIndex]).IsEqualTo((byte)(vyValue - vxValue));
        await Assert.That(cpuContext.Registers.Vf).IsEqualTo(carry);
    }

    [Test]
    public async Task Execute_ShouldShiftRightVxRegister_WhenShiftRightVxVariantInstructionIsSuccessfullyExecuted()
    {
        var vxIndex = 0;
        var vxValue = (byte)0x01; // Binary representation: 00000001
        var vfResultValue = true; // Should be set as the LSB of vxValue, in this case the bit 1 (true)
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.Vf = false;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x80, 0x06]), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue >> 1));
        await Assert.That(cpuContext.Registers.Vf).IsEqualTo(Convert.ToBoolean(vfResultValue));
    }

    [Test]
    public async Task Execute_ShouldShiftLeftVxRegister_WhenShiftLeftVxVariantInstructionIsSuccessfullyExecuted()
    {
        var vxIndex = 0;
        var vxValue = (byte)0x02; // Binary representation: 00000010
        var vfResultValue = false; // Should be set as the LSB of vxValue, in this case the bit 0 (false)
        var cpuContext = new CpuContext();
        cpuContext.Registers.V[vxIndex] = vxValue;
        cpuContext.Registers.Vf = true;
        var sut = new InstructionExecutorImpl();

        sut.Execute(new XyVariantInstruction([0x80, 0x0E]), cpuContext);

        await Assert.That(cpuContext.Registers.V[vxIndex]).IsEqualTo((byte)(vxValue << 1));
        await Assert.That(cpuContext.Registers.Vf).IsEqualTo(Convert.ToBoolean(vfResultValue));
    }
}
