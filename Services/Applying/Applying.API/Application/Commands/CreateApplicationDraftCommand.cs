using MediatR;
using Applying.API.Application.Models;
using System.Collections.Generic;

namespace Microsoft.Fee.Services.Applying.API.Application.Commands
{
    public class CreateApplicationDraftCommand : IRequest<ApplicationDraftDTO>
    {
        public string StudentId { get; private set; }

        public IEnumerable<BasketItem> Items { get; private set; }

        public CreateApplicationDraftCommand(string studentId, IEnumerable<BasketItem> items)
        {
            StudentId = studentId;
            Items = items;
        }
    }
}
