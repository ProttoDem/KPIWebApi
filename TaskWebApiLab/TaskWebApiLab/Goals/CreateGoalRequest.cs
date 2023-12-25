using System.ComponentModel.DataAnnotations;
using TaskWebApiLab.Core.GoalAggregate;

namespace TaskWebApiLab.Goals
{
    public class CreateGoalRequest
    {
        public const string Route = "/Goals";

        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public int ParentGoalId { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public DateTime? DueTime { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
