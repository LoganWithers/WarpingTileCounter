namespace WarpingCounter.Common.Builders.Interfaces
{

    public interface IFromEastGadgetBuilder
    {

        IFromUpGadgetBuilder Up(string name = null);


        IFromDownGadgetBuilder Down(string name = null);


        IFromEastGadgetBuilder East(string name = null);


        IFromNorthGadgetBuilder North(string name = null);


        IFromSouthGadgetBuilder South(string name = null);



        IFromEastGadgetBuilder East(int n);


        IFromNorthGadgetBuilder North(int n);


        IFromSouthGadgetBuilder South(int n);

        IGadgetBuilder End();

    }

}
