
using MudEstoquePersistenciaDados.Models;

namespace MudEstoquePersistenciaDados.Services.Interfaces
{
    public interface IProdutoService
    {
        // Lista todos os produtos em ordem alfabética
        Task<IEnumerable<Produto>> ListarTodosAsync();

        // Busca uma categoria pelo Id; devolve null quando não existe
        Task<Produto?> ObterPorIdAsync(int id);

        // Inclui a categoria e devolve o Id gerado pelo banco
        Task<int> AdicionarAsync(Produto produto);

        // Altera a categoria; devolve false quando o Id não existe
        Task<bool> AlterarAsync(Produto produto);

        // Exclui a categoria; devolve false quando o Id não existe
        Task<bool> ExcluirAsync(int id);
    }
}