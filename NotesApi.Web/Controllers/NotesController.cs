using Microsoft.AspNetCore.Mvc;
using NotesApi.Application.DTO;
using MediatR;
using NotesApi.Application.Commands.CreateNote;
using NotesApi.Application.Commands.DeleteNote;
using NotesApi.Application.Commands.UpdateNote;
using NotesApi.Application.Queries.GetAllNotes;
using NotesApi.Application.Queries.GetNoteById;
using Microsoft.AspNetCore.Authorization;

namespace NotesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllNotesQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NoteDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteCreateDto noteDto)
        {
            var dto = await _mediator.Send(new CreateNoteCommand
            {
                Title = noteDto.Title,
                Content = noteDto.Content
            });

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NoteUpdateDto noteDto)
        {
            await _mediator.Send(new UpdateNoteCommand
            {
                Id = id,
                Title = noteDto.Title,
                Content = noteDto.Content
            });

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteNoteCommand{ Id = id });
            return NoContent();
        }
    }
}
