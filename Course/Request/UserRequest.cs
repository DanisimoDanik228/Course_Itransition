namespace Course.Request
{
    public class UserRequest
    {
        public string Email { get; set; }
        public IEnumerable<string> Role { get; set; }
    }
}
