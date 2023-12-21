using Data.Models;

namespace TaskWebApiLab.ApiModels
{
    public class GoalModel
    {
        public int CategoryId { get; set; }
        public int? ParentTaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        //public Status Status { get; set; } = Status.OnHold;
    }
}
