using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Infrastructure.Data;
using NotesApi.Domain.Models;
using Mapster;
using NotesApi.Application.DTO;
using System.Collections.Generic;
using MediatR;
using NotesApi.Application.Commands;

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
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] NoteCreateDto noteDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(new CreateNoteCommand
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
            });

            var dto = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            if (dto == null) return NotFound();

            Console.WriteLine($"===DTO Id: {dto.Id}===");

            return CreatedAtAction(nameof(GetByIdAsync), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NoteUpdateDto noteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateNoteCommand
            {
                Id = id,
                Title = noteDto.Title,
                Content = noteDto.Content
            });

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _mediator.Send(new DeleteNoteCommand{ Id = id });
            return result ? NoContent() : NotFound();
        }
    }
}
