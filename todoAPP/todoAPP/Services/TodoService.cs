using Microsoft.EntityFrameworkCore;
using todoAPP.Enums;
using todoAPP.Models;
using todoAPP.Models.DTO;
using todoAPP.Models.RequestModel;
using todoAPP.Models.ViewModel;

namespace todoAPP.Services
{
    public interface ITodoService
    {
        public Task<IEnumerable<TodoViewModel>> GetGeneralTodoList(Guid userId);
        public Task<Guid> CreateTodo(CreateTodoDTO model);
        public Task UpdateTodoStatus(Guid todoId);
        public Task UpdateTodoInfo(PatchTodoInfoDTO model);
        public Task UpdateTodoTag(PatchTodoTagDTO model);
        public Task UpdateGeneralTodoOrder(PatchGeneralTodoOrderDTO model);
        public Task UpdateSwimlaneTodoOrder(PatchSwimlaneTodoOrderDTO model);
        public Task DeleteAlreadyDoneTodo(DeleteAlreadyDoneTodoDTO model);
        public Task DeleteTodo(Guid todoId);
    }

    public class TodoService : ITodoService
    {
        private readonly DBContext _dbContext;

        public TodoService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TodoViewModel>> GetGeneralTodoList(Guid userId)
        {
            return await _dbContext.Todo
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.GeneralTodoPosition)
                .Select(x => new TodoViewModel
                {
                    Uid = x.Uid,
                    Status = x.Status,
                    Title = x.Title,
                    Description = x.Description,
                    CreateAt = x.CreateAt,
                    UpdateAt = x.UpdateAt,
                    ExecuteAt = x.ExecuteAt,
                    Position = x.GeneralTodoPosition,
                    Tag = new UserTagViewModel
                    {
                        Uid = x.Tag.Uid,
                        Type = x.Tag.Type,
                        Name = x.Tag.Name,
                    }
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateTodo(CreateTodoDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            var todoId = Guid.NewGuid();

            var swimlane = await _dbContext.KanbanSwimlane
                .Where(x => x.Kanban.UserId == model.UserId && x.Type == (byte)EKanbanSwimlaneType.DEFAULT)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
            var maxGeneralTodoPosition = await _dbContext.Todo
                .Where(x => x.UserId == model.UserId)
                .OrderByDescending(x => x.GeneralTodoPosition)
                .Select(x => x.GeneralTodoPosition)
                .FirstOrDefaultAsync();
            var maxSwimlaneTodoPosition = await _dbContext.Todo
                .Where(x => x.KanbanSwimlaneId == swimlane.Uid)
                .OrderByDescending(x => x.SwimlaneTodoPosition)
                .Select(x => x.SwimlaneTodoPosition)
                .FirstOrDefaultAsync();

            if (model.TagId == Guid.Empty)
            {
                model.TagId = await _dbContext.UserTag
                    .Where(x => x.UserId == model.UserId && x.Type == (byte)EUserTagType.DEFAULT)
                    .Select(x => x.Uid)
                    .SingleOrDefaultAsync();
            }

            await _dbContext.Todo.AddAsync(new Todo
            {
                Uid = todoId,
                Status = (byte)ETodoStatus.UNDONE,
                Title = model.Title,
                Description = model.Description,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
                ExecuteAt = model.ExecuteAt.ToUniversalTime(),
                GeneralTodoPosition = maxGeneralTodoPosition + 1,
                SwimlaneTodoPosition = maxSwimlaneTodoPosition + 1,
                TagId = model.TagId,
                KanbanSwimlaneId = swimlane.Uid,
                UserId = model.UserId,
            });

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return todoId;
        }

        public async Task UpdateTodoStatus(Guid todoId)
        {
            var tx = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Todo
                .Where(x => x.Uid == todoId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, x => x.Status == (byte)ETodoStatus.UNDONE ? (byte)ETodoStatus.DONE : (byte)ETodoStatus.UNDONE)
                    .SetProperty(y => y.UpdateAt, DateTimeOffset.UtcNow));

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task UpdateTodoInfo(PatchTodoInfoDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Todo
                .Where(x => x.Uid == model.TodoId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Title, model.Title)
                    .SetProperty(x => x.Description, model.Description)
                    .SetProperty(x => x.ExecuteAt, model.ExecuteAt)
                    .SetProperty(x => x.UpdateAt, DateTimeOffset.UtcNow));

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task UpdateTodoTag(PatchTodoTagDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            if (model.TagId == Guid.Empty)
            {
                model.TagId = await _dbContext.UserTag
                    .Where(x => x.UserId == model.UserId && x.Type == (byte)EUserTagType.DEFAULT)
                    .Select(x => x.Uid)
                    .SingleOrDefaultAsync();
            }

            await _dbContext.Todo
                .Where(x => x.Uid == model.TodoId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.TagId, model.TagId)
                    .SetProperty(x => x.UpdateAt, DateTimeOffset.UtcNow));

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task UpdateGeneralTodoOrder(PatchGeneralTodoOrderDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            var todo = await _dbContext.Todo
                .Where(x => x.Uid == model.DragTodoId)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

            decimal newOrder;
            if (model.DropPrevTodoId == null && model.DropNextTodoId != null)
            {
                var dropNextTodoPosition = await _dbContext.Todo.Where(x => x.Uid == model.DropNextTodoId).Select(x => x.GeneralTodoPosition).SingleOrDefaultAsync();
                newOrder = dropNextTodoPosition / 2;
            }
            else if (model.DropPrevTodoId != null && model.DropNextTodoId == null)
            {
                var dropPrevTodoPosition = await _dbContext.Todo.Where(x => x.Uid == model.DropPrevTodoId).Select(x => x.GeneralTodoPosition).SingleOrDefaultAsync();
                newOrder = dropPrevTodoPosition + 1;
            }
            else if (model.DropPrevTodoId != null && model.DropNextTodoId != null)
            {
                var prevAndNextTodoPositionSum = await _dbContext.Todo
                    .Where(x => x.Uid == model.DropPrevTodoId || x.Uid == model.DropNextTodoId)
                    .Select(x => x.GeneralTodoPosition)
                    .SumAsync();

                newOrder = prevAndNextTodoPositionSum / 2;
            }
            else
            {
                throw new ArgumentException();
            }
            todo.GeneralTodoPosition = newOrder;
            await _dbContext.SaveChangesAsync();

            var afterPoint = newOrder - Decimal.Floor(newOrder);
            if (afterPoint < 0.0625M && afterPoint > 0)
            {
                var generalTodoList = await _dbContext.Todo
                .Where(x => x.UserId == model.UserId)
                .OrderBy(x => x.GeneralTodoPosition)
                .ToListAsync();

                var count = 1;
                foreach (var todoOrder in generalTodoList)
                {
                    todoOrder.GeneralTodoPosition = count++;
                }
            }

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task UpdateSwimlaneTodoOrder(PatchSwimlaneTodoOrderDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            var todo = await _dbContext.Todo
                .Where(x => x.Uid == model.TodoId)
                .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

            decimal newOrder;
            if (model.DropPrevTodoId == null && model.DropNextTodoId == null)
            {
                newOrder = 1;
            }
            else if (model.DropPrevTodoId == null && model.DropNextTodoId != null)
            {
                var nextTodo = await _dbContext.Todo
                    .Where(x => x.Uid == model.DropNextTodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                newOrder = nextTodo.SwimlaneTodoPosition / 2;
            }
            else if (model.DropPrevTodoId != null && model.DropNextTodoId == null)
            {
                var prevTodo = await _dbContext.Todo
                    .Where(x => x.Uid == model.DropPrevTodoId)
                    .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                newOrder = prevTodo.SwimlaneTodoPosition + 1;
            }
            else if (model.DropPrevTodoId != null && model.DropNextTodoId != null)
            {
                var prevAndNextTodoOrderSum = await _dbContext.Todo
                    .Where(x => x.Uid == model.DropPrevTodoId || x.Uid == model.DropNextTodoId)
                    .Select(x => x.SwimlaneTodoPosition)
                    .SumAsync();
                newOrder = prevAndNextTodoOrderSum / 2;
            }
            else
            {
                throw new ArgumentException();
            }

            todo.SwimlaneTodoPosition = newOrder;
            await _dbContext.SaveChangesAsync();

            if (model.KanbanSwimlaneId != null)
            {
                todo.KanbanSwimlaneId = model.KanbanSwimlaneId.Value;
            }

            var afterPoint = newOrder - Decimal.Floor(newOrder);
            if (afterPoint < 0.0625M && afterPoint > 0)
            {
                var swimlaneTodoList = await _dbContext.Todo
                .Where(x => x.KanbanSwimlaneId == model.KanbanSwimlaneId)
                .OrderBy(x => x.SwimlaneTodoPosition)
                .ToListAsync();

                var count = 1;
                foreach (var todoOrder in swimlaneTodoList)
                {
                    todoOrder.SwimlaneTodoPosition = count++;
                }
            }

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task DeleteAlreadyDoneTodo(DeleteAlreadyDoneTodoDTO model)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Todo
                .Where(x => x.UserId == model.UserId && x.Status == (byte)ETodoStatus.DONE)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task DeleteTodo(Guid todoId)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Todo.Where(x => x.Uid == todoId).ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}

