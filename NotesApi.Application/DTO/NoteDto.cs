namespace NotesApi.Application.DTO
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content {  get; set; } = string.Empty;
    }
}
