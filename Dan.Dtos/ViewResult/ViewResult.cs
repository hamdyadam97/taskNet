using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Dtos.ViewResult
{
    public class ViewResult<TEntity>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public TEntity Entity { get; set; }
    }
}
