namespace todoAPP.RequestModel
{
  public class PaginationRequestModel
  {
    public int? Page { get; set; } = 1;

    public int? Limit { get; set; } = 10;
  }
}