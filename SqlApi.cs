using Npgsql;
using System.Data;
namespace ToDoGraphical

{
    class SqlApi
    {
        string cs;
        NpgsqlCommand cmd;
        NpgsqlConnection con;
        public  SqlApi()
         : this("localhost","postgres","computer","sql_test")
        { }

        public  SqlApi(string host, string username, string passwrd, string database){

            cs = $"Host={host};Username={username};Password={passwrd};database={database}";
        
            con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT version()";

            cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
            
        }
        public void openConnection(){
            con.Open();
            cmd = new NpgsqlCommand();
            cmd.Connection = con;
        }
        public void dropTable(string name){
            
            cmd.CommandText = $"DROP TABLE IF EXISTS {name}";
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Table {name} dropped");
        }
        public void createTable(string nameOfTable, string variablesWithType){
            cmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {nameOfTable}(id SERIAL PRIMARY KEY,{variablesWithType})";
            cmd.ExecuteNonQuery();
            
        }
        public void insertRow(string nameOfTable,string variables,string values){

            cmd.CommandText = $@"INSERT INTO {nameOfTable}({variables}) VALUES ({values})";
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Npgsql.PostgresException){
                Console.WriteLine("Table does not exist");
            }
            

        }
        public void universalCmd(string command){
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
        }
        public void readTable(string name){
            cmd.CommandText = $"SELECT * FROM {name}";
            NpgsqlDataReader rdr = cmd.ExecuteReader();
          //  Console.WriteLine($"{rdr.GetName(0),-15}\n {rdr.GetName(1),-15}\n {rdr.GetName(2),10} \n {rdr.GetName(3),10}");
            while (rdr.Read())
                {
                    Console.Write(rdr.GetName(0)+": ");
                    Console.WriteLine(rdr.GetInt32(0));

                    Console.Write(rdr.GetName(1)+": ");
                    Console.WriteLine(rdr.GetString(1));

                    Console.Write(rdr.GetName(2)+": ");
                    Console.WriteLine(rdr.GetString(2));

                    Console.Write(rdr.GetName(3)+": ");
                    Console.WriteLine(rdr.GetDate(3));
                    Console.WriteLine();
                }

        }
        public void deleteRow(string tableName, int id){
             cmd.CommandText = $"DELETE * FROM {tableName} WHERE id = @id";
             cmd.Parameters.AddWithValue("id", id);
        }
        public List<Task> readTasks(string tableName,DateTime date){
            cmd.CommandText = $"SELECT * FROM {tableName} WHERE date = @date";
            cmd.Parameters.AddWithValue("date", date);

            NpgsqlDataReader reader = cmd.ExecuteReader();
            List<Task> tasks = new List<Task>(); 
            int counter = 0;
            while (reader.Read())
            {
                Task task = convertDataToTask(reader);
                tasks.Add(task);
                Console.WriteLine(counter);
                counter ++;
            }
            return tasks;
        }

        public Task convertDataToTask(NpgsqlDataReader reader){

                int? id = reader["id"] as int?;
                string name = reader["name"] as string;
                string payload = reader["payload"] as string;
                Nullable < DateTime > date = reader["date"] as Nullable < DateTime >;
                Task task = new Task{
                    Id = id.Value,
                    Name = name,
                    Payload = payload,
                    Date = date.Value,
                };
            return task;
        }

        public void updateRow(string tableName,  List<Task> tasks){
            foreach (Task task in tasks){
                updateRow(tableName, task);
            }
        }

        public void updateRow(string tableName, Task task){
            cmd.CommandText = $"UPDATE {tableName} SET name = @name, payload = @payload, date = @date WHERE id = @id";
            cmd.Parameters.AddWithValue("id", task.Id);
            cmd.Parameters.AddWithValue("name",task.Name);
            cmd.Parameters.AddWithValue("payload",task.Payload);
            cmd.Parameters.AddWithValue("date", task.Date);
        }
    }
}