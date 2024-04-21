using TodoAPP_EnumService.Extensions;
using TodoAPP_EnumService.Models;

namespace TodoAPP_EnumService.Helpers;

public static class EnumHelper
{
  public static Task<IEnumerable<TypeViewModel>> GetListAsync<T>() where T : Enum
  {
    return Task.Factory.StartNew(() =>
      {
        return GetList<T>();
      });
  }

  public static IEnumerable<TypeViewModel> GetList<T>() where T : Enum
  {
    return Enum.GetValues(typeof(T))
      .Cast<T>()
      .Select(x => new TypeViewModel
      {
        Key = Convert.ToInt32(x),
        Code = x.ToString(),
        Value = x.GetDescriptionText(),
      });
  }
}
