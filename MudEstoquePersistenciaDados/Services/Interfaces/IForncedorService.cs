using MudEstoquePersistenciaDados.Models;

namespace MudEstoquePersistenciaDados.Services.Interfaces
{
    public interface IFornecedorService
    {
        // Lista todos os fornecedores em ordem alfabética
        Task<IEnumerable<Fornecedor>> ListarTodosAsync();

        // Busca uma categoria pelo Id; devolve null quando não existe
        Task<Fornecedor?> ObterPorIdAsync(int id);

        // Inclui a categoria e devolve o Id gerado pelo banco
        Task<int> AdicionarAsync(Fornecedor fornecedor);

        // Altera a categoria; devolve false quando o Id não existe
        Task<bool> AlterarAsync(Fornecedor fornecedor);

        // Exclui a categoria; devolve false quando o Id não existe
        Task<bool> ExcluirAsync(int id);
    }
}
