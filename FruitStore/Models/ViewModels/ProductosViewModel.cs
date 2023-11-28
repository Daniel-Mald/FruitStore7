namespace FruitStore.Models.ViewModels
{
    public class ProductosViewModel
    {
        public string Categoria { get; set; } = null!;
        public IEnumerable<ProductosModel> Productos { get; set; } = null!;
    }
    public class ProductosModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public string FechaModificacion { get; set; } = null!;
    }
}
