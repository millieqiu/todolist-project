using todoAPP.Models;

namespace todoAPP.Helpers;

public static class UserTagHelper
{
    public static IEnumerable<UserTag> GetDefaultUserTagList(Guid userId)
    {
        for (int i = 1; i < 9; i++)
        {
            yield return new UserTag
            {
                Uid = Guid.NewGuid(),
                Name = $"標籤{i}",
                UserId = userId,
            };
        }
    }
}
