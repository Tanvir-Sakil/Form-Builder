using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domain.DTOs
{
    public class CreateFormDto
    {
        public string Title { get; set; }

        public List<CreateFieldDto> Fields { get; set; }
    }
}
