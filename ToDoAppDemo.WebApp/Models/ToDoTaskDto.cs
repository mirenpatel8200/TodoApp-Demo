using System.ComponentModel.DataAnnotations;

namespace ToDoAppDemo.WebApp.Models
{
    public class ToDoTaskDto
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
