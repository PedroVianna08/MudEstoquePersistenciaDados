using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudEstoquePersistenciaDados.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nome { get; set; } = "";
        [MaxLength(500)]
        public string? Descricao { get; set; }
        [MaxLength(50)]
        public string? CodigoBarras { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; }

        // N:1 com Categoria: FK + navegação
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // N:1 com Fornecedor: FK + navegação
        public int FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }

        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public Estoque? Estoque { get; set; }
        public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = [];

    }
}
