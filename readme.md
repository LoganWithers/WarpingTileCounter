# Thin 3D rectangle + a warping counter

## Figures and such

[Most recent figures and equations](tex/main.pdf)


## Pseudocode/algorithm for initial value
```js
// Group the digits into groups of three from least signifcant to most significant

for (i = 0,...,groups.length - 1) {
    if (i = 0) {
        create seed
        create general seed region
    } else {
        create general seed region
    }
}

i = groups.length

switch (groups[i].digits) {
    case 3 =>  create MSR_3;
    case 2 =>  create MSR_2;
    case 1 =>  create MSR_1;
}
```

## Program for generating tile set


[Tile generator class](WarpingCounter/WarpingCounter/TileGenerator.cs)
