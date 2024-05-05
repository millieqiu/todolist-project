using Microsoft.EntityFrameworkCore;
using todoAPP.DTO;
using todoAPP.Enums;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public interface ITodoListService
    {
        public Task<IEnumerable<TodoViewModel>> GetSortedUserTodoList(GetTodoListDTO model);
        public Task<Guid> CreateTodoItem(CreateTodoDTO model);
        public Task ChangeTodoItemStatus(GeneralRequestModel model);
        public Task UpdateTodoInfo(PatchTodoInfoDTO model);
        public Task ChangeTodoSwimlane(PatchTodoSwimlaneDTO model);
        public Task UpdateUserTodoOrder(PatchUserTodoOrderDTO model);
        public Task UpdateSwimlaneTodoOrder(PatchSwimlaneTodoOrderDTO model);
        public Task DeleteUserAlreadyDoneTodoItem(DeleteUserAlreadyDoneTodoDTO model);
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

        public async Task<IEnumerable<TodoViewModel>> GetSortedUserTodoList(GetTodoListDTO model)
        {
            return await _dbContext.Todo
                .Join(_dbContext.UserTodoOrder
                , x => x.Uid
                , y => y.TodoId
                , (x, y) => new { Todo = x, UserTodoOrder = y })
                .Where(x => x.Todo.UserId == model.UserId)
                .Select(x => new TodoViewModel
                {
                    Uid = x.Todo.Uid,
                    Status = x.Todo.Status,
                    Title = x.Todo.Title,
                    Description = x.Todo.Description,
                    CreateAt = x.Todo.CreateAt,
                    UpdateAt = x.Todo.UpdateAt,
                    ExecuteAt = x.Todo.ExecuteAt,
                    PrevId = x.UserTodoOrder.PrevId,
                    NextId = x.UserTodoOrder.NextId,
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateTodoItem(CreateTodoDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            var todoId = Guid.NewGuid();

            var swimlane = await _dbContext.KanbanSwimlane
                .Where(x => x.Kanban.UserId == model.UserId && x.Type == (byte)EKanbanSwimlaneType.DEFAULT)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
            await _dbContext.Todo.AddAsync(new Todo
            {
                Uid = todoId,
                Status = (byte)ETodoStatus.UNDONE,
                Title = model.Title,
                Description = model.Description,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
                ExecuteAt = model.ExecuteAt.ToUniversalTime(),
                UserId = model.UserId,
                KanbanSwimlaneId = swimlane.Uid,
            });

            var swimlaneTodoMaxOrder = await _dbContext.SwimlaneTodoOrder
                .Include(x => x.Todo)
                .Where(x => x.Todo.KanbanSwimlaneId == swimlane.Uid)
                .OrderByDescending(x => x.Order)
                .Select(x => x.Order)
                .FirstOrDefaultAsync();
            var swimlaneOrder = new SwimlaneTodoOrder
            {
                TodoId = todoId,
                Order = swimlaneTodoMaxOrder + 1,
            };
            await _dbContext.SwimlaneTodoOrder.AddAsync(swimlaneOrder);

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return todoId;
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

                    todoItem.Status = (byte)(todoItem.Status == (byte)ETodoStatus.UNDONE ?
                        ETodoStatus.DONE : ETodoStatus.UNDONE);
                    todoItem.UpdateAt = DateTimeOffset.UtcNow;

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

        public async Task UpdateTodoInfo(PatchTodoInfoDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var todoItem = await _dbContext.Todo
                    .Where(x => x.Uid == model.TodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

                todoItem.Title = model.Title;
                todoItem.Description = model.Description;
                todoItem.ExecuteAt = model.ExecuteAt;
                todoItem.UpdateAt = DateTimeOffset.UtcNow;

                await _dbContext.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task ChangeTodoSwimlane(PatchTodoSwimlaneDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var todo = await _dbContext.Todo
                    .Where(x => x.Uid == model.TodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

                todo.KanbanSwimlaneId = model.KanbanSwimlaneId;

                await _dbContext.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateUserTodoOrder(PatchUserTodoOrderDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // 取得DragTodo和DropTodo本身的資料
                var mainTodo = new List<Guid>(){
                    model.DragTodoId,
                    model.DropTodoId,
                };
                var mainTodoInfo = await _dbContext.UserTodoOrder.Where(x => mainTodo.Contains(x.TodoId)).ToListAsync() ?? throw new KeyNotFoundException();
                var drag = mainTodoInfo.Where(x => x.TodoId == model.DragTodoId).SingleOrDefault() ?? throw new KeyNotFoundException();
                var drop = mainTodoInfo.Where(x => x.TodoId == model.DropTodoId).SingleOrDefault() ?? throw new KeyNotFoundException();

                // 取得DragTodo和DropTodo各自的Prev跟Next的資料
                var relatedTodo = new List<Guid>(){
                    drag.PrevId,
                    drag.NextId,
                    drop.PrevId,
                    drop.NextId,
                };
                var relatedTodoInfo = await _dbContext.UserTodoOrder.Where(x => relatedTodo.Contains(x.TodoId)).ToListAsync() ?? throw new KeyNotFoundException();
                var dragPrev = relatedTodoInfo.Where(x => x.TodoId == drag.PrevId).FirstOrDefault();
                var dragNext = relatedTodoInfo.Where(x => x.TodoId == drag.NextId).FirstOrDefault();
                var dropPrev = relatedTodoInfo.Where(x => x.TodoId == drop.PrevId).FirstOrDefault();
                var dropNext = relatedTodoInfo.Where(x => x.TodoId == drop.NextId).FirstOrDefault();

                // 替換drag、drop、dragPrev和dragNext的Prev跟Next
                if (dragPrev != null) dragPrev.NextId = drag.NextId;
                if (dragNext != null) dragNext.PrevId = drag.PrevId;

                if (model.Action == EUpdateTodoOrderAction.UP)
                {
                    drag.PrevId = drop.PrevId;
                    drag.NextId = drop.TodoId;

                    if (dropPrev != null) dropPrev.NextId = drag.TodoId;
                    drop.PrevId = drag.TodoId;
                }
                else
                {
                    drag.PrevId = drop.TodoId;
                    drag.NextId = drop.NextId;

                    if (dropNext != null) dropNext.PrevId = drag.TodoId;
                    drop.NextId = drag.TodoId;
                }

                await _dbContext.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateSwimlaneTodoOrder(PatchSwimlaneTodoOrderDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            var order = await _dbContext.SwimlaneTodoOrder
                .Include(x => x.Todo)
                .Where(x => x.TodoId == model.TodoId)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

            decimal newOrder;
            if (model.DropPrevTodoId == null && model.DropNextTodoId == null)
            {
                newOrder = 1;
            }
            else if (model.DropPrevTodoId == null && model.DropNextTodoId != null)
            {
                var nextTodoOrder = await _dbContext.SwimlaneTodoOrder
                    .Where(x => x.TodoId == model.DropNextTodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                newOrder = nextTodoOrder.Order / 2;
            }
            else if (model.DropPrevTodoId != null && model.DropNextTodoId == null)
            {
                var prevTodoOrder = await _dbContext.SwimlaneTodoOrder
                    .Where(x => x.TodoId == model.DropPrevTodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                newOrder = prevTodoOrder.Order + 1;
            }
            else if (model.DropPrevTodoId != null && model.DropNextTodoId != null)
            {
                var prevAndNextTodoOrderSum = await _dbContext.SwimlaneTodoOrder
                    .Where(x => x.TodoId == model.DropPrevTodoId || x.TodoId == model.DropNextTodoId)
                    .Select(x => x.Order)
                    .SumAsync();
                newOrder = prevAndNextTodoOrderSum / 2;
            }
            else
            {
                throw new ArgumentException();
            }

            order.Order = newOrder;
            await _dbContext.SaveChangesAsync();

            var afterPoint = newOrder - Decimal.Floor(newOrder);
            if (afterPoint < 0.0625M && afterPoint > 0)
            {
                var swimlaneTodoList = await _dbContext.SwimlaneTodoOrder
                .Where(x => x.Todo.KanbanSwimlaneId == order.Todo.KanbanSwimlaneId)
                .OrderBy(x => x.Order)
                .ToListAsync();

                var count = 1;
                foreach (var todoOrder in swimlaneTodoList)
                {
                    todoOrder.Order = count++;
                }
            }

            if (model.KanbanSwimlaneId != null)
            {
                var todo = await _dbContext.Todo
                    .Where(x=>x.Uid == model.TodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                todo.KanbanSwimlaneId = model.KanbanSwimlaneId.Value;
            }

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task DeleteUserAlreadyDoneTodoItem(DeleteUserAlreadyDoneTodoDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            var todoListQuery = _dbContext.Todo.Where(x => x.UserId == model.UserId && x.Status == (byte)ETodoStatus.DONE);
            var todoIdList = await todoListQuery.Select(x => x.Uid).ToListAsync();
            _dbContext.RemoveRange(
                _dbContext.UserTodoOrder.Where(x => todoIdList.Contains(x.TodoId))
            );
            _dbContext.RemoveRange(todoListQuery);
            await _dbContext.SaveChangesAsync();

            await tx.CommitAsync();
        }

        public async Task DeleteTodoItem(GeneralRequestModel model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            _dbContext.SwimlaneTodoOrder.RemoveRange(
                _dbContext.SwimlaneTodoOrder.Where(x => x.TodoId == model.Uid));
            _dbContext.Todo.RemoveRange(
                _dbContext.Todo.Where(x => x.Uid == model.Uid));

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}

