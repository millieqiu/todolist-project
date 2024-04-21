namespace TodoAPP_EnumService.Models;

public class TypeInfoViewModel
{
  public string Type { get; set; } = string.Empty;
  
  public string Remark { get; set; } = string.Empty;

  public IEnumerable<TypeViewModel> TypeList { get; set; } = new List<TypeViewModel>();
}

public class TypeViewModel
{
  public int Key { get; set; }

  public string Code { get; set; } = string.Empty;

  public string Value { get; set; } = string.Empty;

}

