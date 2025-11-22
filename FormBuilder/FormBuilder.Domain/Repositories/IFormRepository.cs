using FormBuilder.Domain.DTOs;
using FormBuilder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Repositories
{
    public interface IFormRepository
    {
        Task<int> SaveFormAsync(CreateFormDto dto, System.Data.IDbTransaction transaction = null);
        Task<(int totalCount, IEnumerable<FormListDto> items)> GetFormsPagedAsync(int start, int length, string search);
        Task<FormDetails> GetFormDetailsAsync(int formId);
    }
}
