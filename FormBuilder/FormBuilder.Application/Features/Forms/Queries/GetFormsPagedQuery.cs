using FormBuilder.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.Features.Forms.Queries
{
    public record GetFormsPagedQuery(int Start, int Length, string Search) : IRequest<(int totalCount, IEnumerable<FormListDto> items)>;
}
