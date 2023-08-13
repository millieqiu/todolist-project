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

        public bool IsRole(int userId, ERole role)
        {
            return _db.Users
                .Where(user => user.ID == userId && user.Role == role)
                .Any();
        }

        public void EditUserRole(User user, Models.ERole role)
        {
            user.Role = role;
            _db.SaveChanges();
        }
    }
}

