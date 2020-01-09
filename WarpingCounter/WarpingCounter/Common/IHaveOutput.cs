namespace WarpingCounter.Common
{
    using Models;

    /// <summary>
    /// Used to expose the last tile of a gadget's internal tiles, basically so that
    /// it can be attached to the first tile of a different gadget.
    /// </summary>
    public interface IHaveOutput
    {
        Tile Output { get; }
    }
}
