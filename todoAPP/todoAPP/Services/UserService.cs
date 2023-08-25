using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using todoAPP.Models;

namespace todoAPP.Services
{
    public class UserService
    {
        private readonly DataContext _db;
        private readonly AuthService _auth;
        private readonly AvatarService _avartar;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext db, AuthService auth, AvatarService avatar, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _auth = auth;
            _avartar = avatar;
            _httpContextAccessor = httpContextAccessor;
        }

        public User? HasUser(int id)
        {
            return _db.Users
                .Where(x => x.ID == id)
                .SingleOrDefault();
        }

        public User? HasUser(string username)
        {
            return _db.Users
                .Where(x => x.Username == username)
                .SingleOrDefault();
        }

        public void CreateUser(string username, string password, string nickname)
        {
            byte[] salt = _auth.CreateSalt();

            User user = new User
            {
                Username = username,
                Password = _auth.PasswordGenerator(password, salt),
                Nickname = nickname,
                Salt = Convert.ToBase64String(salt),
                Role = Models.ERole.USER,
                Avatar = "default.jpeg"
            };

            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public bool EditUsername(User user, string username)
        {
            if(_db.Users.Any(user => user.Username == username))
            {
                return false;
            }

            user.Username = username;
            _db.SaveChanges();
            return true;
        }

        public void EditNickname(User user, string nickname)
        {
            user.Nickname = nickname;
            _db.SaveChanges();
        }

        public void ChangePassword(User user, string newPassword)
        {
            byte[] salt = _auth.CreateSalt();
            user.Password = _auth.PasswordGenerator(newPassword, salt);
            user.Salt = Convert.ToBase64String(salt);
            _db.SaveChanges();
        }

        public Stream? GetAvatar(string fileName)
        {
            return _avartar.ReadFile(fileName);
        }

        async public Task EditAvatar(User user, IFormFile avatar)
        {
            if(string.IsNullOrEmpty(user.Avatar) == false && user.Avatar != "default.jpeg")
            {
                _avartar.RemoveFile(user.Avatar);
            }
            string fileName = await _avartar.WriteFile(avatar);
            user.Avatar = fileName;
            _db.SaveChanges();
        }

        async public Task SignInUser(HttpContext context, User user)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid,user.ID.ToString()), //使用者ID
                new Claim(ClaimTypes.NameIdentifier,user.Username),  //使用者帳號
                new Claim(ClaimTypes.Name,user.Nickname),  //使用者名稱
                new Claim("Avatar",user.Avatar)  //使用者圖像
            };

            if (user.Role == ERole.ADMIN)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = userPrincipal;
            var props = new AuthenticationProperties();
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, props);
        }

        public int GetUserId()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string Sid = claims.First().Value;
                Int32.TryParse(Sid, out int userId);
                return userId;
            }
            return 0;
        }
    }
}

