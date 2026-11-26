using System.ComponentModel.DataAnnotations;
using MudEstoquePersistenciaDados.Models;

namespace MudEstoquePersistenciaDados.ViewModels
{
    public class FornecedorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe a razão social.")]
        [MaxLength(200, ErrorMessage = "Use no máximo 200 caracteres.")]
        public string RazaoSocial { get; set; } = "";

        [Required(ErrorMessage = "Informe o CNPJ.")]
        [MaxLength(18, ErrorMessage = "Use no máximo 18 caracteres.")]
        public string CNPJ { get; set; } = "";

        // RegularExpression aceita vazio (o campo é opcional); [EmailAddress] rejeitaria ""
        [MaxLength(200, ErrorMessage = "Use no máximo 200 caracteres.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Informe um e-mail válido.")]
        public string? Email { get; set; }

        [MaxLength(20, ErrorMessage = "Use no máximo 20 caracteres.")]
        public string? Telefone { get; set; }

        [MaxLength(300, ErrorMessage = "Use no máximo 300 caracteres.")]
        public string? Endereco { get; set; }

        public bool Ativo { get; set; } = true;

        // Id 0 = inclusão; outro valor = edição
        public bool EmEdicao => Id != 0;

        // Banco -> tela (edição)
        // Copia os dados da entidade para o formulário (edição)
        // Vai do Banco de Dados para a tela (Processo de Edição do Registro)
        public static FornecedorViewModel DaEntidade(Fornecedor fornecedor)
        {
            FornecedorViewModel viewModel = new FornecedorViewModel();
            viewModel.Id = fornecedor.Id;
            viewModel.RazaoSocial = fornecedor.RazaoSocial;
            viewModel.CNPJ = fornecedor.CNPJ;
            viewModel.Email = fornecedor.Email;
            viewModel.Telefone = fornecedor.Telefone;
            viewModel.Endereco = fornecedor.Endereco;
            viewModel.Ativo = fornecedor.Ativo;
            return viewModel;
        }

        // Vai da Tela para o Banco para salvar o que acabou de ser preenchido
        // Monta a entidade que vai para o serviço
        public Fornecedor ParaEntidade()
        {
            Fornecedor fornecedor = new Fornecedor();
            fornecedor.Id = Id;
            fornecedor.RazaoSocial = RazaoSocial?.Trim() ?? string.Empty;
            fornecedor.CNPJ = CNPJ?.Trim() ?? string.Empty;
            fornecedor.Email = TextoOuNulo(Email);
            fornecedor.Telefone = TextoOuNulo(Telefone);
            fornecedor.Endereco = TextoOuNulo(Endereco);
            fornecedor.Ativo = Ativo;
            return fornecedor;
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
