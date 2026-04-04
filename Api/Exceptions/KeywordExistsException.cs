
namespace Api.Exceptions
{
    public class KeywordExistsException : Exception
    {
        public KeywordExistsException() : base("Keyword already exists") {}
        public KeywordExistsException(string message) : base(message) {}
    }
}