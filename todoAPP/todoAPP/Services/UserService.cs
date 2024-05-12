using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using todoAPP.DTO;
using todoAPP.Enums;
using todoAPP.Helpers;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Services;

public interface IUserService
{
    public Guid GetUserId();
    public string GetClaim(string type);
    public Task<UserInfoViewModel> GetUserInfo(GeneralRouteRequestModel model);
    public Task<User> QueryUser(string username);
    public Task CreateUser(RegisterRequestModel model);
    public Task UpdateUsername(PatchUsernameDTO model);
    public Task UpdateNickname(PatchNicknameDTO model);
    public Task ChangePassword(PatchPasswordDTO model);
    public Task<string> GetAvatar();
    public Task UpdateAvatar(PatchAvatarDTO model);
    public Task UpdateUserRole(PatchRoleRequestModel model);
    public Task SignInUser(HttpContext context, User user);
    public Task<bool> CheckUsernameDuplicated(string username);
}

public class UserService : IUserService
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

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Sid)
            ?? throw new UnauthorizedAccessException();
        return new Guid(userId);
    }

    public string GetClaim(string type)
    {
        var identity = _httpContextAccessor.HttpContext!.User.Identity ?? throw new UnauthorizedAccessException("Identity not found");
        var claimsIdentity = new ClaimsIdentity(identity).FindFirst(type) ?? throw new UnauthorizedAccessException("Identity not found");
        return claimsIdentity.Value ?? throw new UnauthorizedAccessException("Identity not found");
    }

    public async Task<UserInfoViewModel> GetUserInfo(GeneralRouteRequestModel model)
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

    public async Task CreateUser(RegisterRequestModel model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

        if (await CheckUsernameDuplicated(model.Username))
            throw new ArgumentException("使用者名稱已存在");

        var salt = PasswordHelper.CreateSalt();
        var userId = Guid.NewGuid();
        await _dbContext.User.AddAsync(new User
        {
            Uid = userId,
            Username = model.Username,
            Password = PasswordHelper.GeneratePassword(model.Password, salt),
            Nickname = model.Nickname,
            Salt = Convert.ToBase64String(salt),
            Role = (int)ERole.USER,
            Avatar = ""
        });

        await CreateUserInitKanban(new InitKanbanDTO
        {
            Name = "Default",
            SwimlaneName = "New",
            UserId = userId
        });
        await CreateUserInitUserTag(userId);

        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    }

    private async Task CreateUserInitKanban(InitKanbanDTO model)
    {
        var kanbanId = Guid.NewGuid();
        await _dbContext.Kanban.AddAsync(new Kanban
        {
            Uid = kanbanId,
            Name = model.Name,
            UserId = model.UserId,
        });
        await _dbContext.KanbanSwimlane.AddAsync(new KanbanSwimlane
        {
            Uid = Guid.NewGuid(),
            Name = model.SwimlaneName,
            Type = (byte)EKanbanSwimlaneType.DEFAULT,
            Position = 0,
            KanbanId = kanbanId,
        });
    }

    private async Task CreateUserInitUserTag(Guid userId)
    {
        await _dbContext.UserTag.AddRangeAsync(UserTagHelper.GetDefaultUserTagList(userId));
    }

    public async Task UpdateUsername(PatchUsernameDTO model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

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

    public async Task UpdateNickname(PatchNicknameDTO model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

        var user = await _dbContext.User
            .Where(x => x.Uid == model.UserId)
            .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

        user.Nickname = model.Nickname;

        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task ChangePassword(PatchPasswordDTO model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

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

    public async Task<string> GetAvatar()
    {
        return await _dbContext.User
            .Where(x => x.Uid == GetUserId())
            .Select(x => x.Avatar)
            .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
    }

    public async Task UpdateAvatar(PatchAvatarDTO model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

        var user = await _dbContext.User
            .Where(x => x.Uid == GetUserId())
            .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

        _avartar.RemoveFile(user.Avatar);
        user.Avatar = await _avartar.WriteFile(model.File);

        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task UpdateUserRole(PatchRoleRequestModel model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

        var user = await _dbContext.User
            .Where(x => x.Uid == model.UserID)
            .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
        user.Role = (int)model.RoleID;
        
        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task SignInUser(HttpContext context, User user)
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

    public async Task<bool> CheckUsernameDuplicated(string username)
    {
        return await _dbContext.User.AnyAsync(x => x.Username == username);
    }
}