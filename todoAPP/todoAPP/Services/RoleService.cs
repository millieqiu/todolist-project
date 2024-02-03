using Microsoft.EntityFrameworkCore;
using todoAPP.Models;
using todoAPP.RequestModel;

namespace todoAPP.Services
{
    public class RoleService
    {
        private readonly DBContext _dbContext;

        public RoleService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateUserRole(PatchRoleRequestModel model)
        {
            using (var tx = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _dbContext.User
                        .Where(x => x.Uid == model.UserID)
                        .SingleOrDefaultAsync() ?? throw new KeyNotFoundException();
                    user.Role = (int)model.RoleID;
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
    }
}

