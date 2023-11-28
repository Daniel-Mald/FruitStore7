namespace FruitStore.Areas.Jirafa.Models
{
    public class AdminCategoriasViewModel
    {
        public IEnumerable<CategoriaModel> Categorias { get; set; } = null!;
    }
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
