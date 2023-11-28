using FruitStore.Models.Entities;

namespace FruitStore.Areas.Jirafa.Models
{
    public class AdminAgregarProductosViewModel
    {
        public IEnumerable<CategoriaModel>? Categorias { get; set; }
        public Productos Productos { get; set; } = new();
        public IFormFile? Archivo { get; set; } 
    }
}
