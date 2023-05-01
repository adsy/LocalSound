using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localsound.backend.Domain.Model.Exception
{
    public class ValidatorException: SystemException
    {
        public Dictionary<string,string[]> Errors { get; set; }

        public ValidatorException(Dictionary<string,string[]> errors)
        {
            Errors = errors;
        }
    }
}
