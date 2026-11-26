using System.ComponentModel.DataAnnotations;

namespace MudEstoquePersistenciaDados.Models
{
    public class Fornecedor
    {
        [Key]
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        [Required, MaxLength(200)]
        public string CNPJ { get; set; }
        [Required, MaxLength(18)]

        public string Email { get; set; }
        [MaxLength(200), EmailAddress]
        public string Telefone { get; set; }
        [MaxLength(20)]
        public string Endereco { get; set; }
        [MaxLength(300)]
        public bool Ativo { get; set; } = true;

        // Lado "um" do 1:N: não vira coluna
        public ICollection<Produto> Produtos
        { get; set; } = [];
    }
}

