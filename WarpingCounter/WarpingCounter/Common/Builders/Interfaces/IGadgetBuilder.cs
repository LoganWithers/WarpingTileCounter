namespace WarpingCounter.Common.Builders.Interfaces
{

    using System.Collections.Generic;

    using Models;

    public interface IGadgetBuilder
    {

        IFromNorthGadgetBuilder North(string color = "white", string name = null);


        IFromSouthGadgetBuilder South(string color = "white", string name = null);


        IFromEastGadgetBuilder East(string color = "white", string name = null);


        IFromWestGadgetBuilder West(string color = "white", string name = null);


        IFromWestGadgetBuilder West(int numberOfTiles, string color = "white");


        IFromEastGadgetBuilder East(int numberOfTiles, string color = "white");


        IFromNorthGadgetBuilder North(int numberOfTiles, string color = "white");


        IFromSouthGadgetBuilder South(int numberOfTiles, string color = "white");


        IGadgetBuilder StartWith(Tile tile);


        IGadgetBuilder Start();

        IGadgetBuilder SouthLine(int numberOfBits);


        IGadgetBuilder NorthLine(int numberOfBits);


        IEnumerable<Tile> Tiles();

    }

}
