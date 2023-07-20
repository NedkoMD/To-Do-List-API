namespace ToDoList.Business.Validations
{
    public class Validations
    {
        public bool IsValidStatus(int status)
        {
            return status >= 0 && status <= 2;
        }

        public bool IsValidPriority(int priorityId)
        {
            return priorityId >= 1 && priorityId <= 3;
        }
    }
}
