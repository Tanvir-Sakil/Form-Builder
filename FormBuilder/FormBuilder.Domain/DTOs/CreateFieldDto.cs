using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domain.DTOs
{
    public class CreateFieldDto
    {
        public string? Label { get; set; }

        public bool IsRequired { get; set; }

        public string? SelectedValue { get; set; }

        public int OrderNo { get; set; }
    }
}
