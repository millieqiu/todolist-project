using System.ComponentModel;

namespace TodoAPP_EnumService.Extensions;

public static class DescriptionAttributeExtensions
{
  public static string GetDescriptionText<T>(this T source)
  {
    var ssource = source!.ToString()!;
    var fieldInfo = source.GetType().GetField(ssource);
    if(fieldInfo != null){
      var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
      if(attributes.Length >0){
        return attributes[0].Description;
      }
    }
    return ssource;
  }
}
