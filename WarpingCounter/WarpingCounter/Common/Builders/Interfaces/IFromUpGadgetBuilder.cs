namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromUpGadgetBuilder
    {

        IFromEastGadgetBuilder East(string name = null);


        IFromWestGadgetBuilder West(string name = null);


        IFromSouthGadgetBuilder South(string name = null);


        IFromNorthGadgetBuilder North(string name = null);


        IGadgetBuilder End();

    }

}
