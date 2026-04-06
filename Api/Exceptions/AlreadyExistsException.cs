
namespace Api.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base("Ressource already exists") {}
        public AlreadyExistsException(string message) : base(message) {}
    }
}