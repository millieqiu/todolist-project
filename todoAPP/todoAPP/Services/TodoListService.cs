using Microsoft.EntityFrameworkCore;
using todoAPP.Extensions;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public interface ITodoListService
    {
        public Task<IEnumerable<TodoViewModel>> GetTodoList(GetTodoListModel model);
        public Task<Guid> CreateTodoItem(CreateTodoModel model);
        public Task ChangeTodoItemStatus(GeneralRequestModel model);
        public Task DeleteTodoItem(GeneralRequestModel model);
    }

    public class TodoListService : ITodoListService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly DBContext _dbContext;
        private readonly WeatherService _weather;

        public TodoListService(IHttpContextAccessor accessor, DBContext dbContext, WeatherService weather)
        {
            _accessor = accessor;
            _dbContext = dbContext;
            _weather = weather;
        }

        public async Task<IEnumerable<TodoViewModel>> GetTodoList(GetTodoListModel model)
        {
            return await _dbContext.Todo
                .AsNoTracking()
                .Where(x => x.UserId == model.UserId)
                .Select(x => new TodoViewModel
                {
                    Uid = x.Uid,
                    Status = x.Status,
                    Text = x.Text,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Weather = x.Weather
                })
                .ToPaginatedListAsync(model, _accessor.HttpContext!);
        }

        public async Task<Guid> CreateTodoItem(CreateTodoModel model)
        {
            var todo = new Todo
            {
                Uid = Guid.NewGuid(),
                Status = 0,
                Text = model.Text,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                Weather = await _weather.GetWeatherText() ?? "",
                UserId = model.UserId,
            };

            await _dbContext.Todo.AddAsync(todo);

            await _dbContext.SaveChangesAsync();

            return todo.Uid;
        }

        public async Task ChangeTodoItemStatus(GeneralRequestModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var todoItem = await _dbContext.Todo
                        .Where(x => x.Uid == model.Uid)
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

                    if (todoItem.Status == 0)
                    {
                        todoItem.Status = 1;
                    }
                    else
                    {
                        todoItem.Status = 0;
                    }
                    todoItem.UpdatedAt = DateTime.Now;

                    await _dbContext.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteTodoItem(GeneralRequestModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var todoItem = await _dbContext.Todo
                        .Where(x => x.Uid == model.Uid)
                        .SingleOrDefaultAsync();
                    if (todoItem != null)
                    {
                        _dbContext.Todo.Remove(todoItem);
                        await _dbContext.SaveChangesAsync();
                    }
                    await tx.CommitAsync();
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
        }
    }
}

