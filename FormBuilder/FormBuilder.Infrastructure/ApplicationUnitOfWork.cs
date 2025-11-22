using FormBuilder.Domain;
using FormBuilder.Infrastructure;
using FormBuilder.Domain.Entities;
using FormBuilder.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormBuilder.Infrastructure.Data;

namespace FormBuilder.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IFormRepository FormRepository { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }

        public ApplicationUnitOfWork(
            ApplicationDbContext dbContext,
            IFormRepository formRepository,
            IUnitOfWork unitOfWork)
            : base(dbContext)
        {
            FormRepository = formRepository;
            UnitOfWork = unitOfWork;
        }
    }
}