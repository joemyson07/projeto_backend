using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplicacaoBackAula.Models;
using AplicacaoBackAula.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AplicacaoBackAula.Controllers
{

    [Route("api[controller]")]
    [ApiController]
    public class ProdutoItemController : ControllerBase
    {
        //declacação do atributo _context
        private readonly BdAulaBackendContext _context;

        //declaração do método Construtor
        public ProdutoItemController(BdAulaBackendContext context)
        {
            //instancia do atributo _context
            _context = context;
        }


        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            var produto = await _context.Produtos
                .Include(p => p.IditemNavigation)
                .ToListAsync();
            return Ok(produto);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos(int id)
        {

            var produto = await _context.Produtos
                .Include(p => p.IditemNavigation)
                .FirstOrDefaultAsync(p => p.Iditem == id);

            if (produto == null)
            {
                return NotFound(
                    new
                    {
                        mensagem = "Produto não localizado"

                    });
            }
            return Ok(produto);
        }

        [HttpPost]

        public async Task<ActionResult> PostProduto(ProdutoDTO dados)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var item = new Item
                {
                    Idcategoria = dados.IdCategoria,

                    Item1 = dados.Item,

                    DescricaoItem = dados.DescricaoItem,

                    Preco = dados.Preco,

                    TipoItem = dados.TipoItem,

                    AtivoProdutoServico = dados.AtivoProdutoServico
                };

                _context.Items.Add(item);

                await _context.SaveChangesAsync();

                int idItemGerado = item.Iditem;



                var produto = new Produto
                {
                    Iditem = idItemGerado,

                    CodigoPreco = dados.CodigoPreco,

                    PrecoVenda = dados.PrecoVenda,

                    Unidade = dados.Unidade
                };

                _context.Produtos.Add(produto);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetProdutos),
                    new { id = produto.Iditem },
                    produto
                    );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    mensagem = "Erro ao cadastrar produto",
                    erro = ex.Message,
                });
            }

        }
        [HttpPut("{id}")]

        public async Task<IActionResult> PutProduto(int id, ProdutoDTO dados)
        {
            var produtosExistentes = await _context.Produtos
                .Include(p => p.IditemNavigation)
                .FirstOrDefaultAsync(p => p.Iditem == id);

            if (produtosExistentes == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado"
                });
            }

            produtosExistentes.IditemNavigation.Idcategoria = dados.IdCategoria;

            produtosExistentes.IditemNavigation.Item1 = dados.Item;

            produtosExistentes.IditemNavigation.DescricaoItem = dados.DescricaoItem;

            produtosExistentes.IditemNavigation.Preco = dados.Preco;

            produtosExistentes.IditemNavigation.TipoItem = dados.TipoItem;

            produtosExistentes.IditemNavigation.AtivoProdutoServico = dados.AtivoProdutoServico;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutosExists(id)) { 
                    
                    return NotFound(new
                    
                    {
                        mensagem = "Produto não encontrado"
                    });
                }
                throw;

            }
            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduto(int id)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p => p.Iditem == id);

                if (produto == null)
                {
                    return NotFound(new
                    {
                        mensagem = "Produto não encontrado"
                    });
                }
                _context.Produtos.Remove(produto);

                await _context.SaveChangesAsync();

                var item = await _context.Items
                       .FirstOrDefaultAsync(i => i.Iditem == id);

                if (item == null)
                {
                    _context.Items.Remove(item);

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    mensagem = "Erro ao excluir produto"
                });
            }
        }

        private bool ProdutosExists(int id)
        {
            return _context.Produtos.Any(p => p.Iditem == id);
        }
    }
}
