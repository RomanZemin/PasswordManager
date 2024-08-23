namespace PassManager.Domain.Entities
{
    public class PasswordRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
