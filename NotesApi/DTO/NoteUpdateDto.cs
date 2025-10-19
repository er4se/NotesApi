using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTO
{
    public class NoteUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;
    }
}
