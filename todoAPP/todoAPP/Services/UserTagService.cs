using Microsoft.EntityFrameworkCore;
using todoAPP.Models;

namespace todoAPP.Services;

public interface IUserTagService
{
    public Task<IEnumerable<UserTagViewModel>> GetUserTagList(GetUserTagListDTO model);
}

public class UserTagService : IUserTagService
{
    private readonly DBContext _dbContext;

    public UserTagService(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserTagViewModel>> GetUserTagList(GetUserTagListDTO model)
    {
        return await _dbContext.UserTag
            .Where(x => x.UserId == model.UserId)
            .Select(x => new UserTagViewModel
            {
                Uid = x.Uid,
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
