using Npgsql;

namespace ToDoGraphical
{
    class Task
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Payload {get;set;}
        public DateTime Date {get; set;}



        public string ToString(){
            
        return $"Id: {Id} \nName: {Name} \nPayload: {Payload} \nDate: {Date} \n-------------------------------------- ";

        }
    }
}
    