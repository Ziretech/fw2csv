namespace FixWidth2Csv
{
    public interface IReader
    {
        bool MoreLines { get; }
        string ReadLine(int minimalNumberOfCharacters);
    }
}
