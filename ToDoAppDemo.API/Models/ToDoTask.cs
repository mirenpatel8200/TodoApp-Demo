using System.ComponentModel.DataAnnotations;

namespace ToDoAppDemo.API.Models
{
    public class ToDoTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Description { get; set; }
    }
}
