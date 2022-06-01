
namespace ToDoGraphical
{
    class Controller{
        List<Task> tasks = new List<Task>();

        public Controller()

        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Window());
            SqlApi sql = new SqlApi();
            // sql.openConnection();
            // sql.dropTable("todo");
            // sql.createTable("todo", @"name VARCHAR(50) NOT NULL, payload  VARCHAR(255) , date DATE NOT NULL");
            // sql.insertRow("todo","name, payload, date","'finish bp','check grammar, update Bibliography and format document', '2023-12-15'");
            // sql.readTable("todo");
            DateTime date = new DateTime(2023, 08, 30); // 2022, 06, 02 funguje

            tasks = sql.readTasks("todo", date);
            foreach (var task in tasks)
            {
                Console.WriteLine(task.ToString());
            }
        }
    }
}
