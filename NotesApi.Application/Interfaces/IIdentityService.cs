using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NotesApi.Application.Common;

namespace NotesApi.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<Guid>> RegisterAsync(string email, string password);
        Task<Result<string>> LoginAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
    }
}
