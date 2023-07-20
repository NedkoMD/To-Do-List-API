namespace ToDoList.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public short Status { get; set; }

        //Foreign key to Priority
        public int PriorityId { get; set; }
        public Priority Priority { get; set; }
    }

    public enum ItemStatus
    {
        Pending,
        InProgress,
        Complete
    }
}
