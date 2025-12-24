using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Common
{
    public static class CacheKeys
    {
        public static class Notes
        {
            public const string All = "notes:all:v1";
            public static string ById(Guid id) => $"notes:{id}";
            public static string ByUser(Guid userId) => $"users:{userId}:notes";
        }
    }
}
