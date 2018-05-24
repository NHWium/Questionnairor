using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Builder.Models
{
    public class ResponseList
    {
        public Guid? ResponseId { get; set; }
        public IEnumerable<Response> Responses { get; set; }
    }
}
