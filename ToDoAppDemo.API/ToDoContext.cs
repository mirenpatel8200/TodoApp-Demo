using Microsoft.EntityFrameworkCore;
using ToDoAppDemo.API.Models;

namespace ToDoAppDemo.API
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
                
        }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}
