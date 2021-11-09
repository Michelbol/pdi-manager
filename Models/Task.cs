namespace PdiManager.Models
{
    public class Task
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsDone { get; set; }
        
        public Pdi Pdi { get; set; }
    }
}