using Microsoft.EntityFrameworkCore;
using todoAPP.Enums;
using todoAPP.Models;
using todoAPP.Models.DTO;
using todoAPP.Models.ViewModel;

namespace todoAPP.Services;

public interface IUserTagService
{
    public Task<IEnumerable<UserTagViewModel>> GetUserTagList(Guid userId);
    public Task PatchUserTagName(PatchUserTagDTO model);
}

public class UserTagService : IUserTagService
{
    private readonly DBContext _dbContext;

    public UserTagService(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserTagViewModel>> GetUserTagList(Guid userId)
    {
        return await _dbContext.UserTag
            .Where(x => x.UserId == userId)
            .Select(x => new UserTagViewModel
            {
                Uid = x.Uid,
                Type = x.Type,
                Name = x.Name,
                Color = x.Color,
            })
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public async Task PatchUserTagName(PatchUserTagDTO model)
    {
        using var tx = await _dbContext.Database.BeginTransactionAsync();

        foreach (var tag in model.TagList)
        {
            await _dbContext.UserTag
                .Where(x => x.Uid == tag.Uid && x.Type == (byte)EUserTagType.GENERAL && x.UserId == model.UserId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Name, tag.Name));
        }

        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    }
}
