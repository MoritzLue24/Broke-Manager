
namespace Api.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Ressource not found") {}
        public NotFoundException(string message) : base(message) {}
    }
}