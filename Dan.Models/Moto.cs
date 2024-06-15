using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Models
{
    public class Moto
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Range(0, 9999999999.99)]
        public decimal Price { get; set; } = 5000;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Range(0, int.MaxValue)]
        public int Total { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int Available { get; set; } = 1;
    }
}
