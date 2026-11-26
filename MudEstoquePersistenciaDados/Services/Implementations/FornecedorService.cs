using Microsoft.EntityFrameworkCore;
using MudEstoquePersistenciaDados.Data;
using MudEstoquePersistenciaDados.Models;
using MudEstoquePersistenciaDados.Services.Interfaces;

namespace MudEstoquePersistenciaDados.Services.Implementations
{
    public class FornecedorService : IFornecedorService
    {

        private readonly IDbContextFactory<EstoqueDbContext> _fabrica;

        // Injeção de dependência (o "D" do SOLID): o serviço declara do que precisa no
        // construtor e o contêiner entrega a fábrica registrada no Program.cs
        public FornecedorService(IDbContextFactory<EstoqueDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        public async Task<int> AdicionarAsync(Fornecedor fornecedor)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            bool cnpjEmUso = await contexto.Fornecedores.AnyAsync(f => f.CNPJ == fornecedor.CNPJ);
            if (cnpjEmUso)
            {
                throw new InvalidOperationException($"Já existe um fornecedor com o CNPJ {fornecedor.CNPJ}");
            }
            contexto.Fornecedores.Add(fornecedor);
            await contexto.SaveChangesAsync();
            return fornecedor.Id;
        }

        public async Task<bool> AlterarAsync(Fornecedor fornecedor)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            Fornecedor? fornecedorExistente = await contexto.Fornecedores.FindAsync(fornecedor.Id);
            if (fornecedorExistente == null)
            {
                return false;
            }
            bool cnpjEmUso = await contexto.Fornecedores.AnyAsync(f => f.CNPJ == fornecedor.CNPJ && f.Id != fornecedor.Id);
            if (cnpjEmUso)
            {
                throw new InvalidOperationException($"Já existe um fornecedor com o CNPJ {fornecedor.CNPJ}");
            }
            fornecedorExistente.RazaoSocial = fornecedor.RazaoSocial;
            fornecedorExistente.CNPJ = fornecedor.CNPJ;
            fornecedorExistente.Email = fornecedor.Email;
            fornecedorExistente.Telefone = fornecedor.Telefone;
            fornecedorExistente.Endereco = fornecedor.Endereco;
            fornecedorExistente.Ativo = fornecedor.Ativo;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            Fornecedor? fornecedorExistente = await contexto.Fornecedores.FindAsync(id);
            if (fornecedorExistente == null)
            {
                return false;
            }
            bool temProdutos = await contexto.Produtos.AnyAsync(p => p.FornecedorId == id);
            if (temProdutos)
            {
                throw new InvalidOperationException($"Este fornecedor tem produtos e não pode ser excluído. Exclua primeiro os produtos relacionados.");
            }

            contexto.Fornecedores.Remove(fornecedorExistente);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Fornecedor>> ListarTodosAsync()
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            return await contexto.Fornecedores.AsNoTracking().OrderBy(f => f.RazaoSocial).ToListAsync();
        }

        public async Task<Fornecedor?> ObterPorIdAsync(int id)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            return await contexto.Fornecedores.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}