using Mapster;
using NotesApi.Application.DTO;
using NotesApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Common
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Note, NoteDto>.NewConfig();
            TypeAdapterConfig<NoteCreateDto, Note>.NewConfig();
            TypeAdapterConfig<NoteUpdateDto, Note>.NewConfig();
        }
    }
}
