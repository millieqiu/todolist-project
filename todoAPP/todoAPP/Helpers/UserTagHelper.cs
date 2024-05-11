using todoAPP.Enums;
using todoAPP.Models;

namespace todoAPP.Helpers;

public static class UserTagHelper
{
    public static IEnumerable<UserTag> GetDefaultUserTagList(Guid userId)
    {

        yield return new UserTag
        {
            Uid = Guid.NewGuid(),
            Type = (byte)EUserTagType.DEFAULT,
            Name = "沒有標籤",
            UserId = userId,
        };
        for (int i = 1; i < 9; i++)
        {
            yield return new UserTag
            {
                Uid = Guid.NewGuid(),
                Type = (byte)EUserTagType.GENERAL,
                Name = $"標籤{i}",
                UserId = userId,
            };
        }
    }
}
