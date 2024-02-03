using System.ComponentModel.DataAnnotations;

namespace todoAPP.RequestModel
{
    public class CreateTodoRequestModel
    {
        [MaxLength(30)]
        public string Text { get; set; } = string.Empty;
    }

    public class CreateTodoModel : CreateTodoRequestModel
    {
        public Guid UserId { get; set; }
    }
}