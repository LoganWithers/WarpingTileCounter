namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromEastGadgetBuilder
    {
        
        IFromDownGadgetBuilder Down(string color = "white", string name = null);

        IFromUpGadgetBuilder Up(string color = "white", string name = null);

        IFromSouthGadgetBuilder South(string color = "white", string name = null);

        IFromNorthGadgetBuilder North(string color = "white", string name = null);

        IFromEastGadgetBuilder East(string color = "white", string name = null);


        IFromEastGadgetBuilder East(int n, string color = "white");

        IFromSouthGadgetBuilder South(int n, string color = "white");

        IFromNorthGadgetBuilder North(int n, string color = "white");
        
        IGadgetBuilder End();

    }

}
