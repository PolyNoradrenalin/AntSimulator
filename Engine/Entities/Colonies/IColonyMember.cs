namespace AntEngine.Entities.Colonies
{
    /// <summary>
    ///     Represents a colony member, a class possessing a home colony.
    /// </summary>
    public interface IColonyMember
    {
        Colony Home { get; set; }
    }
}