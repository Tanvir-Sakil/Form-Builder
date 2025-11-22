using FormBuilder.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.Features.Forms.Commands
{
    public record CreateFormCommand(CreateFormDto Dto) : IRequest<int>;
}
