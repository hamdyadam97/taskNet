using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Dtos.ViewResult
{
    public class ListResult<TEntity>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<TEntity> Entities { get; set; }
        public int Count { get; set; }
        public ListResult()
        {
            Entities = new List<TEntity>();
        }
    }
}
