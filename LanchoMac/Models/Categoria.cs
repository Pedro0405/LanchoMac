using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchoMac.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "O Numero m[aximo é de 100 caracteres")]
        [Display(Name = "Nome")]
        public string CategoriaNome { get; set;}
        [Required]
        [StringLength(200, ErrorMessage = "O Numero máximo é de 200 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public List<Lanche> lanches { get; set;}
    }
}
