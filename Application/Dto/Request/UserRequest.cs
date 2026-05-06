namespace Application.Dto.Request
{
    public class UserRequest
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
