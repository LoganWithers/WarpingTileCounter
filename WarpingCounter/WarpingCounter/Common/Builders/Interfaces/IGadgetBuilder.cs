namespace WarpingCounter.Common.Builders.Interfaces
{

    using System.Collections.Generic;

    using Models;

    public interface IGadgetBuilder
    {

        IFromNorthGadgetBuilder North(string name = null);


        IFromSouthGadgetBuilder South(string name = null);


        IFromEastGadgetBuilder East(string name = null);


        IFromWestGadgetBuilder West(string name = null);


        IFromWestGadgetBuilder West(int numberOfTiles);


        IFromEastGadgetBuilder East(int numberOfTiles);


        IFromNorthGadgetBuilder North(int numberOfTiles);


        IFromSouthGadgetBuilder South(int numberOfTiles);


        IGadgetBuilder StartWith(Tile tile);


        IGadgetBuilder Start();


        IGadgetBuilder SouthLine(int numberOfBits);


        IGadgetBuilder NorthLine(int numberOfBits);


        IEnumerable<Tile> Tiles();

    }

}
