using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Infrastructure.Data;
using NotesApi.Domain.Models;
using Mapster;
using NotesApi.Application.DTO;
using System.Collections.Generic;
using MediatR;
using NotesApi.Application.Commands;
using NotesApi.Application.Commands.CreateNote;
using NotesApi.Application.Commands.DeleteNote;
using NotesApi.Application.Commands.UpdateNote;
using NotesApi.Application.Queries.GetAllNotes;
using NotesApi.Application.Queries.GetNoteById;

namespace NotesApi.Controllers
{
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
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllAsync() => Ok(await _mediator.Send(new GetAllNotesQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetByIdAsync(int id)
        {
            var result = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] NoteCreateDto noteDto)
        {
            var id = await _mediator.Send(new CreateNoteCommand
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
            });

            var dto = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            return CreatedAtAction(nameof(GetByIdAsync), new { id = dto!.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NoteUpdateDto noteDto)
        {
            var result = await _mediator.Send(new UpdateNoteCommand
            {
                Id = id,
                Title = noteDto.Title,
                Content = noteDto.Content
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _mediator.Send(new DeleteNoteCommand{ Id = id });
            return NoContent();
        }
    }
}
