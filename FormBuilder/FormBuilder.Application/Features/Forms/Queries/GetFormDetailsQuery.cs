using FormBuilder.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.Features.Forms.Queries
{
    public record GetFormDetailsQuery(int FormId):IRequest<FormDetails>;
}
