namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromSouthGadgetBuilder
    {
        IFromUpGadgetBuilder Up(string name = null);
        IFromDownGadgetBuilder Down(string name = null);
        IFromEastGadgetBuilder East(string name = null);
        IFromWestGadgetBuilder West(string name = null);
        IFromSouthGadgetBuilder South(string name = null);
        IGadgetBuilder End();

    }

}