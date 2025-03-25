using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ganets.Domain.Entities
{
    public class Gadget:Entity
    {
        public string Description { get; set; } // описание гаджета
        public decimal Price { get; set; } // цена гаджета
        public string? Image { get; set; } // путь к файлу изображения    

        // Навигационные свойства 
        /// <summary> 
        /// группа гаджетов (например, телефоны, часы) 
        /// </summary> 
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
