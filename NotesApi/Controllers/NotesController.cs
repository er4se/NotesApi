using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Infrastructure.Data;
using NotesApi.Domain.Models;
using NotesApi.Application.Services;
using Mapster;
using NotesApi.Application.DTO;
using System.Collections.Generic;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _service;

        public NotesController(INotesService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<NoteDto>> GetAll() => Ok(_service.GetAll().Adapt<IEnumerable<NoteDto>>());

        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            var result = _service.GetById(id);
            if (result == null) return NotFound();

            return Ok(result.Adapt<NoteDto>());
        }

        [HttpPost]
        public IActionResult Create([FromBody] NoteCreateDto noteDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var note = noteDto.Adapt<Note>();
            var created = _service.Create(note);

            var result = created.Adapt<NoteDto>();
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] NoteUpdateDto noteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var note = noteDto.Adapt<Note>();
            var result = _service.Update(id, note);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            return result ? NoContent() : NotFound();
        }
    }
}
