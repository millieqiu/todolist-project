using Microsoft.EntityFrameworkCore;
using todoAPP.DTO;
using todoAPP.Enums;
using todoAPP.Models;
using todoAPP.ViewModel;

namespace todoAPP.Services;

public interface IKanbanService
{
  public Task InitKanban(InitKanbanDTO model);
  public Task<Guid> GetDefaultKanbanId(GetDefaultKanbanDTO model);
  public Task<IEnumerable<KanbanSwimlaneListViewModel>> GetKanbanSwimlaneList(GetKanbanSwimlaneListDTO model);
  public Task<IEnumerable<KanbanViewModel>> GetKanbanList(GetKanbanListDTO model);
  public Task CreateKanbanSwimlane(CreateKanbanSwimlaneDTO model);
  public Task PatchKanbanSwimlaneName(PatchKanbanSwimlaneNameDTO model);
  public Task DeleteKanbanSwimlane(DeleteKanbanSwimlaneDTO model);
}

public class KanbanService : IKanbanService
{
  private readonly DBContext _dbContext;
  public KanbanService(DBContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task InitKanban(InitKanbanDTO model)
  {
    using var tx = await _dbContext.Database.BeginTransactionAsync();
    try
    {
      var kanbanId = Guid.NewGuid();
      await _dbContext.Kanban.AddAsync(new Kanban
      {
        Uid = kanbanId,
        Name = model.Name,
        UserId = model.UserId,
      });
      await _dbContext.KanbanSwimlane.AddAsync(new KanbanSwimlane
      {
        Uid = Guid.NewGuid(),
        Name = model.SwimlaneName,
        Type = (byte)EKanbanSwimlaneType.DEFAULT,
        KanbanId = kanbanId,
      });
      await _dbContext.SaveChangesAsync();
      await tx.CommitAsync();
    }
    catch (Exception)
    {
      await tx.RollbackAsync();
      throw;
    }
  }

  public async Task<IEnumerable<KanbanViewModel>> GetKanbanList(GetKanbanListDTO model)
  {
    return await _dbContext.Kanban
      .Where(x => x.UserId == model.UserId)
      .Select(x => new KanbanViewModel
      {
        Uid = x.Uid,
        Name = x.Name,
        KanbanSwimlaneList = x.KanbanSwimlane.Select(y => new KanbanSwimlaneViewModel
        {
          Uid = y.Uid,
          Type = y.Type,
          Name = y.Name,
          TodoList = y.Todo.Select(z => new TodoViewModel
          {
            Uid = z.Uid,
            Status = z.Status,
            Text = z.Text,
            CreatedAt = z.CreatedAt,
            UpdatedAt = z.UpdatedAt,
            Weather = z.Weather,
          })
        })
      })
      .ToListAsync();
  }

  public async Task<Guid> GetDefaultKanbanId(GetDefaultKanbanDTO model)
  {
    var defaultKanban = await _dbContext.Kanban
      .Where(x => x.UserId == model.UserId)
      .FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
    return defaultKanban.Uid;
  }

  public async Task<IEnumerable<KanbanSwimlaneListViewModel>> GetKanbanSwimlaneList(GetKanbanSwimlaneListDTO model)
  {
    return await _dbContext.KanbanSwimlane
    .Where(x => x.KanbanId == model.KanbanId)
    .Select(x => new KanbanSwimlaneListViewModel
    {
      Uid = x.Uid,
      Name = x.Name
    })
    .ToListAsync();
  }

  public async Task CreateKanbanSwimlane(CreateKanbanSwimlaneDTO model)
  {
    using var tx = await _dbContext.Database.BeginTransactionAsync();
    try
    {
      await _dbContext.KanbanSwimlane.AddAsync(new KanbanSwimlane
      {
        Uid = Guid.NewGuid(),
        Name = model.Name,
        Type = (byte)EKanbanSwimlaneType.GENERAL,
        KanbanId = model.KanbanId,
      });
      await _dbContext.SaveChangesAsync();
      await tx.CommitAsync();
    }
    catch (Exception)
    {
      await tx.RollbackAsync();
      throw;
    }
  }

  public async Task PatchKanbanSwimlaneName(PatchKanbanSwimlaneNameDTO model)
  {
    using var tx = await _dbContext.Database.BeginTransactionAsync();
    try
    {
      var swimlane = await _dbContext.KanbanSwimlane
        .Where(x => x.Uid == model.KanbanSwimlaneId)
        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
      swimlane.Name = model.Name;
      await _dbContext.SaveChangesAsync();

      await tx.CommitAsync();
    }
    catch (Exception)
    {
      await tx.RollbackAsync();
      throw;
    }
  }

  public async Task DeleteKanbanSwimlane(DeleteKanbanSwimlaneDTO model)
  {
    using var tx = await _dbContext.Database.BeginTransactionAsync();
    try
    {
      var swimlane = await _dbContext.KanbanSwimlane
        .Where(x => x.Uid == model.KanbanSwimlaneId)
        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
      if (swimlane.Type == (byte)EKanbanSwimlaneType.DEFAULT)
      {
        throw new ArgumentException("無法刪除預設的KanbanSwimlane");
      }

      _dbContext.Todo.RemoveRange(
        _dbContext.Todo.Where(x => x.KanbanSwimlaneId == model.KanbanSwimlaneId));
      _dbContext.KanbanSwimlane.Remove(swimlane);
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
