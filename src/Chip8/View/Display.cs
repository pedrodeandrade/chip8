using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace Chip8.View;

public static class Display
{
    private const byte Rows = 32;
    private const byte Columns = 64;
    private const byte BitsPerByte = 8;

    private static readonly BitArray _pixels;

    static Display()
    {
        _pixels = new(Rows * Columns, false);
    }

    public static void SetPixels(byte x, byte y, byte pixels, out bool pixelErased)
    {
        pixelErased = false;

        if (y > Rows - 1 || x > Columns - 1)
            return;

        // Bits are loaded in little-endian format
        BitArray bits = new([pixels]);

        var currentX = x;

        /*
         *  As bytes are loaded in little-endian, I process the pixels bit from end to beginning
         * so the bit matches the right pixel on the display
         */
        for (var i = BitsPerByte - 1; i >= 0; i--)
        {
            var index = GetPixelIndexByDisplayCoords(currentX, y);
            var pixel = _pixels[index];
            var newPixel = pixel ^ bits[i];

            _pixels[index] = newPixel;

            if (!newPixel && pixel)
                pixelErased = true;

            currentX++;
        }
    }

    public static void Render()
    {
        Console.Clear();

        // Pixels + space between them
        var displayContent = new StringBuilder((Columns * 2 - 1) * Rows);

        /*
         * Render row by row because that's the way the pixels are represented in the BitArray
         * so doing it guarantees cache affinity
         */
        for (byte i = 0; i < Rows; i++)
        {
            for (byte j = 0; j < Columns; j++)
            {
                var pixel = _pixels[GetPixelIndexByDisplayCoords(j, i)];

                displayContent.Append(pixel ? "*" : " ");
                if (j != Columns - 1)
                    displayContent.Append(" ");
            }

            displayContent.Append("\n");
        }

        Console.Write(displayContent);
    }

    public static void Clear()
    {
        _pixels.SetAll(false);
        Render();
    }

    /// <summary>
    /// Gets the bit position in the pixels array by the display position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>Pixel bit position in the array</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static short GetPixelIndexByDisplayCoords(byte x, byte y)
        => (short)(y * Columns + x); // y * columns gives us the initial index of the row, then we add x to get the right column in the row
}
