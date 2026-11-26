using Microsoft.EntityFrameworkCore;
using MudEstoquePersistenciaDados.Data;
using MudEstoquePersistenciaDados.Models;
using MudEstoquePersistenciaDados.Services.Interfaces;

namespace MudEstoquePersistenciaDados.Services.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IDbContextFactory<EstoqueDbContext> _fabrica;

        // Injeção de dependência (o "D" do SOLID): o serviço declara do que precisa no
        // construtor e o contêiner entrega a fábrica registrada no Program.cs
        public CategoriaService(IDbContextFactory<EstoqueDbContext> fabrica)
        {
            _fabrica = fabrica;
        }


        public async Task<int> AdicionarAsync(Categoria categoria)
        {
            //Await para tornar a web Asyncona
            await using EstoqueDbContext contexto =await _fabrica.CreateDbContextAsync();
            bool nomeEmUso = await contexto.Categorias.AnyAsync(c=>c.Nome==categoria.Nome);
            if (nomeEmUso)
            {
                throw new InvalidOperationException($"Já existe uma categoria chamada {categoria.Nome}");
            }
            contexto.Categorias.Add(categoria);
            await contexto.SaveChangesAsync();
            return categoria.Id;
        }

        public async Task<bool> AlterarAsync(Categoria categoria)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            Categoria? categoriaExistente = await contexto.Categorias.FindAsync(categoria.Nome);
            if(categoriaExistente == null)
            {

                return false;
            }
            bool nomeEmUso = await contexto.Categorias.AnyAsync(c => c.Nome == categoria.Nome);
            if (nomeEmUso)
            {
                throw new InvalidOperationException($"Já existe uma categoria chamada {categoria.Nome}");
            }
            categoriaExistente.Nome = categoria.Nome;
            categoria.Descricao = categoria.Descricao;
            categoria.Ativo = categoria.Ativo;
            contexto.Categorias.Update(categoriaExistente);
            await contexto.SaveChangesAsync();
            return true;
        }
   
        public async Task<bool> ExcluirAsync(int id)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            Categoria? categoriaExistente = await contexto.Categorias.FindAsync(id);
            if (categoriaExistente == null)
            {
                return false;
            }
            bool temProdutos = await contexto.Produtos.AnyAsync(p => p.Categoria.Id == id);
            if (temProdutos)
            {
                throw new InvalidOperationException($"Essa categoria não pode ser excluída. Exclua primeiro os produtos relacionados.");
            }
            contexto.Categorias.Remove(categoriaExistente);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Categoria>> ListarTodosAsync()
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            return await contexto.Categorias.AsNoTracking().OrderBy(c=>c.Nome).ToListAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            await using EstoqueDbContext contexto = await _fabrica.CreateDbContextAsync();
            return await contexto.Categorias.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
        }
    }
}