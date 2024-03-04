using System.Data;

namespace khdima.Models.type
{
    public class UserRole
    {
        public User_has_role UserHasRole { get; set; }
        public Users User { get; set; }
        public Roles Role { get; set; }

    
    }
}
