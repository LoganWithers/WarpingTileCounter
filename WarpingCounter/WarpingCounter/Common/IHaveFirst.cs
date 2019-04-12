namespace WarpingCounter.Common
{

    using Models;

    /// <summary>
    /// Used to expose the first tile of a gadget's internal tiles, basically so that
    /// it can be attached to the last tile of a different gadget.
    /// </summary>
    public interface IHaveFirst
    {

        Tile First { get; }

    }

}
