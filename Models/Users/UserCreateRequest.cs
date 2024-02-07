namespace BroadcastAPI.Models.Users
{
    public class UserCreateRequest
    {
        public int Id { get; set; }
        public string IdCard { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
    }

    public class UserFilterRequest
    {
        #nullable enable
        public int? Id { get; set; }
        public string? IdCard { get; set; }
        public string? DateOfBirth { get; set; }
    }
}
