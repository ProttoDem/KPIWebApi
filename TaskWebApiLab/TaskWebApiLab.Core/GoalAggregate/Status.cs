using Ardalis.SmartEnum;

namespace TaskWebApiLab.Core.GoalAggregate
{
    /*public class Status : SmartEnum<Status>
    {
        public static readonly Status OnHold = new(nameof(OnHold), 0);
        public static readonly Status InProgress = new(nameof(InProgress), 1);
        public static readonly Status Done = new(nameof(Done), 2);

        protected Status(string name, int value) : base(name, value) { }
    }*/

    public enum Status
    {
        OnHold = 0,
        InProgress = 1,
        Done = 2,
    }
}
