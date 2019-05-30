namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromNorthGadgetBuilder
    {

        IFromUpGadgetBuilder Up(string name = null);


        IFromDownGadgetBuilder Down(string name = null);


        IFromEastGadgetBuilder East(string name = null);


        IFromWestGadgetBuilder West(string name = null);


        IFromNorthGadgetBuilder North(string name = null);
        
        IFromEastGadgetBuilder East(int n);
        
        IFromWestGadgetBuilder West(int n);
        
        IFromNorthGadgetBuilder North(int n);


        IGadgetBuilder End();

    }

}
