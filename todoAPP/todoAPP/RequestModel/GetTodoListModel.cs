namespace todoAPP.RequestModel
{
  public class GetTodoListModel : PaginationRequestModel
  {
    public Guid UserId { get; set; }
  }
}