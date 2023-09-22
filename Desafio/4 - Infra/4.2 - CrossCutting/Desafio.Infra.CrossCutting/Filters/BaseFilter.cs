using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Infra.CrossCutting.Filters
{
    public abstract class BaseFilter
    {
        public int Id { get; set; }
        public string Order { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
