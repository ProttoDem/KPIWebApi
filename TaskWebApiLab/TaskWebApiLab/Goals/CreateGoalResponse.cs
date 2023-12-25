namespace TaskWebApiLab.Goals
{
    public class CreateGoalResponse(int id, string title)
    {
        public int Id { get; set; } = id;
        public string Title { get; set; } = title;
    }
}
