namespace Api.Exceptions
{
    public class InvalidRoleException : Exception
    {
        public InvalidRoleException(string message) : base(message) {}
    }
}