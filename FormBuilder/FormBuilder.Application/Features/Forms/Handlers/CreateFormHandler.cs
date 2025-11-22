using FormBuilder.Application.Features.Forms.Commands;
using FormBuilder.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.Features.Forms.Handlers
{
    public class CreateFormHandler : IRequestHandler<CreateFormCommand,int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IUnitOfWork _uow;

        public CreateFormHandler(IApplicationUnitOfWork applicationUnitOfWork, IUnitOfWork uow)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _uow = uow;
        }
        public async Task<int>Handle (CreateFormCommand request, CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var createdId = await _applicationUnitOfWork.FormRepository.SaveFormAsync(request.Dto, _uow.Transaction);
                await _uow.CommitAsync();
                return createdId;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
