using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoAPP.Models;
using todoAPP.RequestModel;
using todoAPP.ViewModel;

namespace todoAPP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class RoleController : Controller
    {
        private readonly DataContext _db;

        public RoleController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            return Ok(GetPaginatedData());
        }

        [HttpGet]
        public IActionResult ListPagination(int page)
        {
            if (page < 1)
            {
                page = 1;
            }
            return Ok(GetPaginatedData(page));
        }

        [HttpPost]
        public IActionResult Create(CreateRoleRequestModel request)
        {
            if (HasRole(request.Name) != null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "CreateRole",
                    Status = 1,
                    ErrMsg = "Role is already exists.",
                };
                return BadRequest(err);
            }

            int id = CreateRole(request.Name);

            return Ok(new GeneralViewModel() { ID = id });
        }

        [HttpPut]
        public IActionResult Update(ModifyRoleRequestModel request)
        {
            Role? role = HasRole(request.ID);
            if (role == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "UpdateRole",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            EditRole(role,request.Name);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(GeneralRequestModel request)
        {
            Role? role = HasRole(request.ID);
            if (role == null)
            {
                ErrorViewModel err = new ErrorViewModel()
                {
                    Service = "DeleteRole",
                    Status = 1,
                    ErrMsg = "Resource not found",
                };
                return NotFound(err);
            }

            DeleteRole(role);

            return Ok();
        }

        private List<RoleViewModel> GetPaginatedData()
        {
            return _db.Roles
                .Select(x => new RoleViewModel
                {
                    ID = x.ID,
                    Name = x.Name
                }).ToList();
        }

        private List<RoleViewModel> GetPaginatedData(int page)
        {
            return _db.Roles
                .Skip((page - 1) * 10)
                .Take(10)
                .Select(x => new RoleViewModel
                {
                    ID = x.ID,
                    Name = x.Name
                }).ToList();
        }

        private Role? HasRole(int id)
        {
            return _db.Roles
                .Where(x => x.ID == id)
                .SingleOrDefault();
        }

        private Role? HasRole(string roleName)
        {

            return _db.Roles
                .Where(x => x.Name == roleName)
                .SingleOrDefault();
        }

        private int CreateRole(string Name)
        {
            Role item = new Role()
            {
                Name = Name
            };

            var t = _db.Roles.Add(item);

            _db.SaveChanges();

            return t.Entity.ID;
        }

        private void EditRole(Role role, string name)
        {
            role.Name = name;

            _db.SaveChanges();
        }

        private void DeleteRole(Role role)
        {
            _db.Roles.Remove(role);

            _db.SaveChanges();
        }
    }
}

