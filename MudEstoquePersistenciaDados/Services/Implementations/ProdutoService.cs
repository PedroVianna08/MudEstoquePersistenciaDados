using Microsoft.EntityFrameworkCore;
using MudEstoquePersistenciaDados.Data;
using MudEstoquePersistenciaDados.Models;
using MudEstoquePersistenciaDados.Services.Interfaces;

namespace MudEstoquePersistenciaDados.Services.Implementations
{
    public class ProdutoService : IProdutoService
    {
        private readonly IDbContextFactory<EstoqueDbContext> _fabrica;

        // Injeção de dependência (o "D" do SOLID): o serviço declara do que precisa no
        // construtor e o contêiner entrega a fábrica registrada no Program.cs
        public ProdutoService(IDbContextFactory<EstoqueDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        public async Task<int> AdicionarAsync(Produto produto)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();

            // Validação de unicidade do nome do produto
            bool nomeEmUso = await contexto.Produtos.AnyAsync(p => p.Nome == produto.Nome);
            if (nomeEmUso)
            {
                throw new InvalidOperationException($"Já existe um produto chamado {produto.Nome}");
            }

            // Validação de código de barras (caso informado)
            if (!string.IsNullOrWhiteSpace(produto.CodigoBarras))
            {
                bool codigoEmUso = await contexto.Produtos.AnyAsync(p => p.CodigoBarras == produto.CodigoBarras);
                if (codigoEmUso)
                {
                    throw new InvalidOperationException($"Já existe um produto com o código de barras {produto.CodigoBarras}");
                }
            }

            contexto.Produtos.Add(produto);
            await contexto.SaveChangesAsync();
            return produto.Id;
        }

        public async Task<bool> AlterarAsync(Produto produto)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();

            Produto? produtoExistente = await contexto.Produtos.FindAsync(produto.Id);
            if (produtoExistente == null)
            {
                return false;
            }

            // Garante que outro produto não possua o mesmo nome
            bool nomeEmUso = await contexto.Produtos.AnyAsync(p => p.Nome == produto.Nome && p.Id != produto.Id);
            if (nomeEmUso)
            {
                throw new InvalidOperationException($"Já existe um produto chamado {produto.Nome}");
            }

            // Garante que outro produto não possua o mesmo código de barras
            if (!string.IsNullOrWhiteSpace(produto.CodigoBarras))
            {
                bool codigoEmUso = await contexto.Produtos.AnyAsync(p => p.CodigoBarras == produto.CodigoBarras && p.Id != produto.Id);
                if (codigoEmUso)
                {
                    throw new InvalidOperationException($"Já existe um produto com o código de barras {produto.CodigoBarras}");
                }
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.CodigoBarras = produto.CodigoBarras;
            produtoExistente.PrecoUnitario = produto.PrecoUnitario;
            produtoExistente.CategoriaId = produto.CategoriaId;
            produtoExistente.FornecedorId = produto.FornecedorId;
            produtoExistente.Ativo = produto.Ativo;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();

            Produto? produtoExistente = await contexto.Produtos.FindAsync(id);
            if (produtoExistente == null)
            {
                return false;
            }

            // Verifica se existem movimentações associadas (DeleteBehavior.Restrict)
            bool temMovimentacoes = await contexto.Movimentacoes.AnyAsync(m => m.ProdutoId == id);
            if (temMovimentacoes)
            {
                throw new InvalidOperationException("Este produto possui movimentações de estoque registradas e não pode ser excluído.");
            }

            contexto.Produtos.Remove(produtoExistente);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Produto>> ListarTodosAsync()
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            return await contexto.Produtos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            return await contexto.Produtos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .Include(p => p.Estoque)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
