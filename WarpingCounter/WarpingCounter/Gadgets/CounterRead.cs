﻿namespace WarpingCounter.Gadgets
{
    using System;
    using System.Collections.Generic;

    using Common;
    using Common.Models;

    public class CounterRead
    {
        public readonly List<Tile> Tiles;

        public CounterRead(string name, Glue input, Glue outputOne, Glue outputZero)
        {
            var guessOne  = new GuessOne(outputOne);
            var guessZero = new GuessZero(outputZero);

            guessOne.Bind(guessZero);
            guessOne.Input.South = input;

            Tiles = new List<Tile>();
            Tiles.AddRange(guessOne.Tiles);
            Tiles.AddRange(guessZero.Tiles);
            Tiles.RenameWithIndex(name);
        }

        private class GuessOne : IHaveInput, IHaveOutput
        {
            private readonly Tile first;

            private readonly Tile fourth;

            private readonly Tile second;

            private readonly Tile third;

            public readonly List<Tile> Tiles;

            public GuessOne(Glue output)
            {
                first  = new Tile($"GuessOne {Guid.NewGuid()}");
                second = new Tile($"GuessOne {Guid.NewGuid()}");
                third  = new Tile($"GuessOne {Guid.NewGuid()}");
                fourth = new Tile($"GuessOne {Guid.NewGuid()}");

                Input        = first;
                Output       = fourth;
                Output.North = output;

                AttachInternalGlues();
                Tiles = new List<Tile> {first, second, third, fourth};
            }

            public Tile Input { get; }

            public Tile Output { get; }

            public void Bind(IHaveInput zero)
            {
                Input.AttachAbove(zero.Input);
            }

            private void AttachInternalGlues()
            {
                first.AttachNorth(second);
                second.AttachNorth(third);
                third.AttachNorth(fourth);
            }
        }

        private class GuessZero : IHaveInput, IHaveOutput
        {
            private readonly Tile fifth;

            private readonly Tile first;

            private readonly Tile fourth;

            private readonly Tile second;

            private readonly Tile third;

            public readonly List<Tile> Tiles;

            public GuessZero(Glue output)
            {
                first  = new Tile($"GuessZero {Guid.NewGuid()}");
                second = new Tile($"GuessZero {Guid.NewGuid()}");
                third  = new Tile($"GuessZero {Guid.NewGuid()}");
                fourth = new Tile($"GuessZero {Guid.NewGuid()}");
                fifth  = new Tile($"GuessZero {Guid.NewGuid()}");

                Input        = first;
                Output       = fifth;
                Output.North = output;
                AttachInternalGlues();

                Tiles = new List<Tile> {first, second, third, fourth, fifth};
            }

            public Tile Input { get; }

            public Tile Output { get; }

            private void AttachInternalGlues()
            {
                first.AttachNorth(second);
                second.AttachNorth(third);
                third.AttachNorth(fourth);
                fourth.AttachBelow(fifth);
            }
        }
    }
}
