using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Extensions;

public static class PaginationExtension
{
  public static async Task<IEnumerable<T>> ToPaginatedListAsync<T>(this IQueryable<T> query, PaginationRequestModel paginationInfo, HttpContext context)
  {
    var page = paginationInfo.Page ?? 1;
    var limit = paginationInfo.Limit ?? 10;
    var totalCount = await query.CountAsync();
    var totalPages = (int)Math.Ceiling((double)totalCount / limit);

    var pagination = new Pagination
    {
      CurrentPage = page,
      TotalPages = totalPages,
      PageSize = limit,
      TotalCount = totalCount,
      HasPrevious = page > 1,
      HasNext = totalPages - page > 0
    };
    context.Response.Headers.Add("x-pagination", JsonSerializer.Serialize(pagination));

    return await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
  }
}