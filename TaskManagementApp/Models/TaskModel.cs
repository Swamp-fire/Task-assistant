using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class TaskModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public TimeSpan? Duration { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string? Repetition { get; set; } // e.g., "Daily", "Weekly", "Monthly"

        public string? Priority { get; set; } // e.g., "High", "Medium", "Low"

        public string? Category { get; set; }
    }
}
