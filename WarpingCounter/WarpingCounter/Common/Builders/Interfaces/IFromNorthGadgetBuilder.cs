namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromNorthGadgetBuilder
    {

        IFromDownGadgetBuilder Down(string color = "white", string name = null);

        IFromUpGadgetBuilder Up(string color = "white", string name = null);

        IFromWestGadgetBuilder West(string color = "white", string name = null);

        IFromNorthGadgetBuilder North(string color = "white", string name = null);

        IFromEastGadgetBuilder East(string color = "white", string name = null);


        IFromEastGadgetBuilder East(int n, string color = "white");

        IFromWestGadgetBuilder West(int n, string color = "white");

        IFromNorthGadgetBuilder North(int n, string color = "white");
        
        IGadgetBuilder End();


    }

}
