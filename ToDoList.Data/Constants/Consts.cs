namespace ToDoList.Data.Constants
{
    public static class Consts
    {
        public const string dbConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public const string InvalidStatusId = "Invalid status. Status should be between 0 and 2";
        public const string InvalidPriorityId = "Invalid priority. Priority should be between 1 and 3";
        public const string ItemNotFound = "Item not found";
    }
}
