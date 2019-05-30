# Thin 3D rectangle + a warping counter

## Figures and such

[Most recent figures and equations](tex/main.pdf)

```c#
(string bits, bool carryOut) CalculateOutputBits(string inputBits, bool carry, int baseM)
{
    const int binary = 2;

    if (!carry)
    {
        return (inputBits, false);
    }

    // Right most bits, do not impact the value that used by the counter.
    var indicatorBits = inputBits.GetLast(2);

    // The bits relevant to the value of the digit.
    var bits = inputBits.Substring(0, inputBits.Length - 2);

    // Convert these bits to its integer representation.
    var value = Convert.ToInt32(bits, binary);

    bool CanAddOne(int n) => n + 1 <= baseM - 1;

    if (CanAddOne(value))
    {
        var incremented = Convert.ToString(value + 1, binary)
                                 .PadLeft(bits.Length, '0');

        // take the newly incremented value, and
        // set the carry signal to false since we incremented in this row.
        return ($"{incremented}{indicatorBits}", false);
    }

    // Set all the bits to 0 since adding a value to the current value will
    // result in some value that is not less than base M. Propagate the
    // carry signal.
    var zeroes = string.Concat(Enumerable.Repeat("0", bits.Length));

    return ($"{zeroes}{indicatorBits}", true);
}
```

## Program for generating tile set


[Tile generator class](WarpingCounter/WarpingCounter/TileGenerator.cs)
