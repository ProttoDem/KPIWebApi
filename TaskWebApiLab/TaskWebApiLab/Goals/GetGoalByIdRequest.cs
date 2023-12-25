namespace TaskWebApiLab.Goals
{
    public class GetGoalByIdRequest
    {
        public const string Route = "/Goals/{GoalId:int}";

        public static string BuildRoute(int goalId) => Route.Replace("{GoalId:int}", goalId.ToString());

        public int GoalId { get; set; }
    }
}
