namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromWestGadgetBuilder
    {

        IFromUpGadgetBuilder Up(string name = null);


        IFromDownGadgetBuilder Down(string name = null);


        IFromWestGadgetBuilder West(string name = null);


        IFromNorthGadgetBuilder North(string name = null);


        IFromSouthGadgetBuilder South(string name = null);


        IGadgetBuilder End();

    }

}
