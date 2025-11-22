using FormBuilder.Application.Features.Forms.Queries;
using FormBuilder.Domain;
using FormBuilder.Domain.DTOs;
using FormBuilder.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.Features.Forms.Handlers
{
    public class GetFormsPagedHandler : IRequestHandler<GetFormsPagedQuery, (int totalCount,IEnumerable<FormListDto> items)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetFormsPagedHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(int totalCount,IEnumerable<FormListDto>items)> Handle(GetFormsPagedQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.FormRepository.GetFormsPagedAsync(request.Start, request.Length, request.Search);
        }
    }
}
