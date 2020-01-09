# Warping Counter

## [Write up of the construction](tex/main.pdf)

## [Implementation of the write up](WarpingCounter/WarpingCounter/TileGenerator.cs)

## Usage

This program generates 3D tile sets designed to be used in either [PyTAS](http://self-assembly.net/wiki/index.php?title=PyTAS) or [ISU TAS](http://self-assembly.net/wiki/index.php?title=ISU_TAS).

The produced tile sets are programmed to self-assemble into thin rectangles
at temperature-1 in the aTAM model. The terminal assembly will be a N * K rectangle.

Once the program is running, it will prompt the user for a height (N)
and width (k) for the rectangle.

After the user has specified their desired dimensions, a `.tds` file will be written into this `WarpingTileCounter/WarpingCounter/Output/` directory.

## Run the program

[Windows x64](WarpingCounter\WarpingCounter\bin\Release\netcoreapp3.0\publish\win-x64)

[Windows x86](WarpingCounter\WarpingCounter\bin\Release\netcoreapp3.0\publish\win-x86)

[Mac](WarpingCounter\WarpingCounter\bin\Release\netcoreapp3.0\publish\mac)

[Linux](WarpingCounter\WarpingCounter\bin\Release\netcoreapp3.0\publish\linux)