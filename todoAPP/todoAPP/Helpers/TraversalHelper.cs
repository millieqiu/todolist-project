using todoAPP.ViewModel;

namespace todoAPP.Helpers;

public static class TraversalHelper
{
  // 最大遞迴次數限制
  public static IEnumerable<T> Traversal<T>(T first, Dictionary<Guid, T> dict) where T: TodoViewModel
  {
    return FindNext<T>(dict, first);
  }

  public static IEnumerable<T> FindNext<T>(Dictionary<Guid, T> dict, T current) where T: TodoViewModel
  {
    var ordered = new List<T>
    {
        current
    };
    if (current.NextId != Guid.Empty)
    {
      return ordered.Concat(FindNext(dict, dict[current.NextId]));
    }
    else
    {
      return ordered;
    }
  }
}
