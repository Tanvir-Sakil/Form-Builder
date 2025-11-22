using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Entities
{
    public class FormDetails
    {
        public int FormId { get; set; }

        public string Title { get; set; }

        public List<Form> Fields {  get; set; } = new List<Form>();

        public FormDetails()
        {
            Fields = new List<Form>();
        }
    }
}
