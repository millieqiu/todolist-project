using Microsoft.EntityFrameworkCore;
using todoAPP.DTO;
using todoAPP.Enums;
using todoAPP.Models;
using todoAPP.ViewModel;

namespace todoAPP.Services;

public interface IKanbanService
{
	public Task<Guid> GetDefaultKanbanId(GetDefaultKanbanDTO model);
	public Task<IEnumerable<KanbanSwimlaneListViewModel>> GetKanbanSwimlaneList(GetKanbanSwimlaneListDTO model);
	public Task<IEnumerable<KanbanViewModel>> GetKanbanList(GetKanbanListDTO model);
	public Task CreateKanbanSwimlane(CreateKanbanSwimlaneDTO model);
	public Task PatchKanbanSwimlaneName(PatchKanbanSwimlaneNameDTO model);
	public Task PatchKanbanSwimlaneOrder(PatchKanbanSwimlaneOrderDTO model);
	public Task DeleteKanbanSwimlane(DeleteKanbanSwimlaneDTO model);
}

public class KanbanService : IKanbanService
{
	private readonly DBContext _dbContext;
	public KanbanService(DBContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IEnumerable<KanbanViewModel>> GetKanbanList(GetKanbanListDTO model)
	{
		return await _dbContext.Kanban
		  .Where(x => x.UserId == model.UserId)
		  .Select(x => new KanbanViewModel
		  {
			  Uid = x.Uid,
			  Name = x.Name,
			  KanbanSwimlaneList = x.KanbanSwimlane
			.OrderBy(y => y.Position)
			.Select(y => new KanbanSwimlaneViewModel
			{
				Uid = y.Uid,
				Type = y.Type,
				Name = y.Name,
				Position = y.Position,
				TodoList = y.Todo
			  .OrderBy(z => z.SwimlaneTodoPosition)
			  .Select(z => new TodoViewModel
			  {
				  Uid = z.Uid,
				  Status = z.Status,
				  Title = z.Title,
				  Description = z.Description,
				  CreateAt = z.CreateAt,
				  UpdateAt = z.UpdateAt,
				  ExecuteAt = z.ExecuteAt,
				  Position = z.SwimlaneTodoPosition,
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

		var maxSwimlanePosition = await _dbContext.KanbanSwimlane
			.Where(x => x.KanbanId == model.KanbanId)
			.OrderByDescending(x => x.Position)
			.Select(x => x.Position)
			.FirstOrDefaultAsync();
		await _dbContext.KanbanSwimlane.AddAsync(new KanbanSwimlane
		{
			Uid = Guid.NewGuid(),
			Name = model.Name,
			Type = (byte)EKanbanSwimlaneType.GENERAL,
			Position = maxSwimlanePosition + 1,
			KanbanId = model.KanbanId,
		});

		await _dbContext.SaveChangesAsync();
		await tx.CommitAsync();
	}

	public async Task PatchKanbanSwimlaneName(PatchKanbanSwimlaneNameDTO model)
	{
		using var tx = await _dbContext.Database.BeginTransactionAsync();

		var swimlane = await _dbContext.KanbanSwimlane
			.Where(x => x.Uid == model.KanbanSwimlaneId)
			.SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
		swimlane.Name = model.Name;

		await _dbContext.SaveChangesAsync();
		await tx.CommitAsync();
	}

	public async Task PatchKanbanSwimlaneOrder(PatchKanbanSwimlaneOrderDTO model)
	{
		using var tx = await _dbContext.Database.BeginTransactionAsync();
		var swimlane = await _dbContext.KanbanSwimlane
			.Where(x => x.Uid == model.DragSwimlaneId)
			.SingleOrDefaultAsync() ?? throw new KeyNotFoundException();

		decimal newOrder;
		if (model.DropPrevSwimlaneId == null && model.DropNextSwimlaneId != null)
		{
			var dropNextSwimlanePosition = await _dbContext.KanbanSwimlane
				.Where(x => x.Uid == model.DropNextSwimlaneId)
				.Select(x => x.Position)
				.SingleOrDefaultAsync();
			newOrder = dropNextSwimlanePosition / 2;
		}
		else if (model.DropPrevSwimlaneId != null && model.DropNextSwimlaneId == null)
		{
			var dropPrevSwimlanePosition = await _dbContext.KanbanSwimlane
				.Where(x => x.Uid == model.DropPrevSwimlaneId)
				.Select(x => x.Position)
				.SingleOrDefaultAsync();
			newOrder = dropPrevSwimlanePosition + 1;
		}
		else if (model.DropPrevSwimlaneId != null && model.DropNextSwimlaneId != null)
		{
			var prevAndNextSwimlanePositionSum = await _dbContext.KanbanSwimlane
				.Where(x => x.Uid == model.DropPrevSwimlaneId || x.Uid == model.DropNextSwimlaneId)
				.Select(x => x.Position)
				.SumAsync();
			newOrder = prevAndNextSwimlanePositionSum / 2;
		}
		else
		{
			throw new ArgumentException();
		}
		swimlane.Position = newOrder;
		await _dbContext.SaveChangesAsync();

		var afterPoint = newOrder - Decimal.Floor(newOrder);
		if (afterPoint < 0.0625M && afterPoint > 0)
		{
			var swimlaneList = await _dbContext.KanbanSwimlane
				.Where(x => x.KanbanId == swimlane.KanbanId)
				.OrderBy(x => x.Position)
				.ToListAsync();

			var count = 1;
			foreach (var swimlaneOrder in swimlaneList)
			{
				swimlaneOrder.Position = count++;
			}
		}

		await _dbContext.SaveChangesAsync();
		await tx.CommitAsync();
	}

	public async Task DeleteKanbanSwimlane(DeleteKanbanSwimlaneDTO model)
	{
		using var tx = await _dbContext.Database.BeginTransactionAsync();

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
}
