using FormBuilder.Application.Features.Forms.Queries;
using FormBuilder.Domain;
using FormBuilder.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.Features.Forms.Handlers
{
    public class GetFormDetailsHandler :IRequestHandler<GetFormDetailsQuery,FormDetails>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetFormDetailsHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<FormDetails>Handle(GetFormDetailsQuery request,CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.FormRepository.GetFormDetailsAsync(request.FormId);
        }
    }
}
