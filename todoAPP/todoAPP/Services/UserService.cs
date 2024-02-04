using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Security.Claims;
using todoAPP.Enums;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public class UserService
    {
        private readonly DBContext _dbContext;
        private readonly AvatarService _avartar;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DBContext dbContext, AvatarService avatar, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _avartar = avatar;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserInfoViewModel> GetUserInfo(GeneralRequestModel model)
        {
            return await _dbContext.User
                .Where(x => x.Uid == model.Uid)
                .Select(x => new UserInfoViewModel
                {
                    Uid = x.Uid,
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Role = x.Role//TODO change to byte
                })
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
        }

        public async Task<User> QueryUser(string username)
        {
            return await _dbContext.User
                .AsNoTracking()
                .Where(x => x.Username == username)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
        }


        public User? HasUser(string username)
        {
            return _dbContext.User
                .Where(x => x.Username == username)
                .SingleOrDefault();
        }

        public async Task CreateUser(RegisterRequestModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var salt = PasswordHelper.CreateSalt();

                    User user = new User
                    {
                        Uid = Guid.NewGuid(),
                        Username = model.Username,
                        Password = PasswordHelper.GeneratePassword(model.Password, salt),
                        Nickname = model.Nickname,
                        Salt = Convert.ToBase64String(salt),
                        Role = (int)ERole.USER,
                        Avatar = ""
                    };

                    await _dbContext.User.AddAsync(user);
                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await tx.RollbackAsync();

                    switch (((SqlException)ex.InnerException!).Number)
                    {
                        case 2627:
                            throw new HttpRequestException("帳號已存在", ex, HttpStatusCode.BadRequest);
                        default:
                            throw;
                    }
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> CheckUsernameDuplicated(string username)
        {
            return await _dbContext.User.AnyAsync(x => x.Username == username);
        }

        public async Task UpdateUsername(PatchUsernameModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (await CheckUsernameDuplicated(model.Username))
                    {
                        throw new DuplicateNameException();
                    }
                    var user = await _dbContext.User
                        .Where(x => x.Uid == model.UserId)
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                    user.Username = model.Username;

                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateNickname(PatchNicknameModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _dbContext.User
                        .Where(x => x.Uid == model.UserId)
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

                    user.Nickname = model.Nickname;

                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task ChangePassword(PatchPasswordModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _dbContext.User
                        .Where(x => x.Uid == model.UserId)
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

                    if (PasswordHelper.ValidatePassword(user.Password, user.Salt, model.OldPassword) == false)
                    {
                        throw new UnauthorizedAccessException();
                    }

                    var salt = PasswordHelper.CreateSalt();

                    user.Password = PasswordHelper.GeneratePassword(model.NewPassword, salt);
                    user.Salt = Convert.ToBase64String(salt);

                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<string> GetAvatar()
        {
            return await _dbContext.User
                .Where(x => x.Uid == GetUserId())
                .Select(x => x.Avatar)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
        }

        async public Task UpdateAvatar(PatchAvatarModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _dbContext.User
                        .Where(x => x.Uid == GetUserId())
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

                    _avartar.RemoveFile(user.Avatar);
                    user.Avatar = await _avartar.WriteFile(model.File);

                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateUserRole(PatchRoleRequestModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _dbContext.User
                        .Where(x => x.Uid == model.UserID)
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                    user.Role = (int)model.RoleID;
                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        async public Task SignInUser(HttpContext context, User user)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid,user.Uid.ToString()), //使用者ID
                new Claim(ClaimTypes.NameIdentifier,user.Username),  //使用者帳號
                new Claim(ClaimTypes.Name,user.Nickname),  //使用者名稱
                new Claim("Avatar",user.Avatar)  //使用者圖像
            };

            if (user.Role == (int)ERole.ADMIN)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(3600),
                IsPersistent = true
            };
            // Thread.CurrentPrincipal = claimsPrincipal;
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
        }

        public Guid GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Sid)
                ?? throw new UnauthorizedAccessException();
            return new Guid(userId);
        }
    }
}

