using System.Data;

namespace AuthAPI.Models
{
    public class UserRoleModel
    {
        public int userId { get; set; }
        public UserModel user { get; set; }
        public int roleId { get; set; }
        public RoleModel role { get; set; }
    }
}
