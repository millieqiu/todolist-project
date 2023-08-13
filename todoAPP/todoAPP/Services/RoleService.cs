using System;
using todoAPP.Models;

namespace todoAPP.Services
{

    public class RoleService
    {
        private readonly DataContext _db;

        public RoleService(DataContext db)
        {
            _db = db;
        }

        public Role? HasRole(string roleName)
        {

            return _db.Roles
                .Where(x => x.Name == roleName)
                .SingleOrDefault();
        }

        public bool CheckUserRole(int userID, string roleName)
        {
            return _db.Users.Where(x => x.Role.Name == roleName && x.ID == userID).Any();
        }

        public void EditUserRole(User user, Role role)
        {
            user.Role = role;
            _db.SaveChanges();
        }
    }
}

