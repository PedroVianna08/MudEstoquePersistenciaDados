using System.ComponentModel.DataAnnotations;
using MudEstoquePersistenciaDados.Models;

namespace MudEstoquePersistenciaDados.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        [MaxLength(100, ErrorMessage = "Use no máximo 100 caracteres.")]
        public string Nome { get; set; } = "";

        [MaxLength(500, ErrorMessage = "Use no máximo 500 caracteres.")]
        public string? Descricao { get; set; }

        [MaxLength(50, ErrorMessage = "Use no máximo 50 caracteres.")]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "Informe o preço unitário.")]
        [Range(0.01, 999999.99, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal PrecoUnitario { get; set; }

        // N:1 com Categoria: FK + navegação
        [Required(ErrorMessage = "Selecione uma categoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma categoria válida.")]
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // N:1 com Fornecedor: FK + navegação
        [Required(ErrorMessage = "Selecione um fornecedor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um fornecedor válido.")]
        public int? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }

        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Id 0 = inclusão; outro valor = edição
        public bool EmEdicao => Id != 0;

        // Copia os dados da entidade para o formulário (edição)
        // Vai do Banco de Dados para a tela (Processo de Edição do Registro)
        public static ProdutoViewModel DaEntidade(Produto produto)
        {
            ProdutoViewModel viewModel = new ProdutoViewModel();
            viewModel.Id = produto.Id;
            viewModel.Nome = produto.Nome;
            viewModel.Descricao = produto.Descricao;
            viewModel.CodigoBarras = produto.CodigoBarras;
            viewModel.PrecoUnitario = produto.PrecoUnitario;
            viewModel.CategoriaId = produto.CategoriaId;
            viewModel.Categoria = produto.Categoria;
            viewModel.FornecedorId = produto.FornecedorId;
            viewModel.Fornecedor = produto.Fornecedor;
            viewModel.Ativo = produto.Ativo;
            viewModel.DataCadastro = produto.DataCadastro;
            return viewModel;
        }

        // Vai da Tela para o Banco para salvar o que acabou de ser preenchido
        // Monta a entidade que vai para o serviço
        public Produto ParaEntidade()
        {
            Produto produto = new Produto();
            produto.Id = Id;
            produto.Nome = Nome?.Trim() ?? string.Empty;
            produto.Descricao = TextoOuNulo(Descricao);
            produto.CodigoBarras = TextoOuNulo(CodigoBarras);
            produto.PrecoUnitario = PrecoUnitario;
            produto.CategoriaId = CategoriaId ?? 0;
            produto.FornecedorId = FornecedorId ?? 0;
            produto.Ativo = Ativo;
            produto.DataCadastro = DataCadastro;
            return produto;
        }

        // Texto em branco vira null: o banco guarda NULL, não ""
        private static string? TextoOuNulo(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            return texto.Trim();
        }
    }
}
