namespace NotesApi.Domain.Models
{
    public class Note
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Note() {}

        public static Note Create(string title, string content)
        {
            return new Note
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
        }
            
        public void Update(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
