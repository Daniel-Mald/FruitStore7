namespace FruitStore.Areas.Jirafa.Models
{
    public class AdminProductosViewModel
    {
        public IEnumerable<CategoriaModel> Categorias { get; set; } = null!;
        public IEnumerable<ProductoModel> Productos { get; set; } = null!;
        public int IdCategoriaSeleccioneada { get; set; }
    }
    public class ProductoModel
    {
        public int Id { get; set; }
        public string Categoria { get; set; } = null!;
        public string Nombre { get; set; } = null!;
    }
    
}
