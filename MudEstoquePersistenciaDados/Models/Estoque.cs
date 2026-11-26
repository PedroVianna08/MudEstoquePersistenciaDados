using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudEstoquePersistenciaDados.Models
{
    public class Estoque
    {
        [Key] public int Id { get; set; }

        // FK única: cada produto tem no máximo um estoque
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public int QuantidadeAtual { get; set; }
        public int EstoqueMinimo { get; set; }
        public int EstoqueMaximo { get; set; } = 9999;
        [MaxLength(100)]
        public string? Localizacao { get; set; }
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;

        // Calculada em memória: não existe no banco
        [NotMapped]
        public bool EstaBaixo => QuantidadeAtual < EstoqueMinimo;
    }
}