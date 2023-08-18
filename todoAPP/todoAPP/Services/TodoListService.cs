using Microsoft.EntityFrameworkCore;
using todoAPP.Models;

namespace todoAPP.Services
{
    public class TodoListService
    {

        private readonly DataContext _db;
        private readonly WeatherService _weather;

        public TodoListService(DataContext db, WeatherService weather)
        {
            _db = db;
            _weather = weather;
        }

        public List<Todo> GetPaginatedData(int page, int userId)
        {
            return _db.TodoList
                .Where(x => x.User.ID == userId)
                .OrderByDescending(item => item.CreatedAt)
                .Skip((page - 1) * 10)
                .Take(10)
                .AsNoTracking()//斷開連結
                .ToList();
        }

        public double GetNumOfPages(int userId)
        {
            int count = _db.TodoList.Where(x => x.User.ID == userId).Count();
            return Math.Ceiling(count / 10.0);
        }

        public Todo? HasItem(int userId, int todoItemId)
        {
            return _db.TodoList
                .Where(x => x.User.ID == userId && x.ID == todoItemId)
                .SingleOrDefault();
        }

        async public Task<int> CreateItem(string text, User user)
        {
            Todo item = new Todo()
            {
                Text = text,
                User = user,
                Weather = await _weather.GetWeatherText(),
            };

            var t = _db.TodoList.Add(item);

            _db.SaveChanges();

            return t.Entity.ID;
        }

        public void ChangeItemStatus(Todo item)
        {
            if (item.Status == 0)
            {
                item.Status = 1;
            }
            else
            {
                item.Status = 0;
            }
            item.UpdatedAt = DateTime.Now;

            _db.SaveChanges();
        }

        public void DeleteItem(Todo item)
        {
            _db.TodoList.Remove(item);

            _db.SaveChanges();
        }
    }
}

