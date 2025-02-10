using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.DTO.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Budget { get; set; }
    }
}
