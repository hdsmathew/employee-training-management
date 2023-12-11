namespace Infrastructure.Entities
{
    public class UserEntity : EntityBase
    {
        public int Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NIC { get; set; }
        public string Phone { get; set; }
        public int DeptID { get; set; }
        public int ManagerID { get; set; }
    }
}
