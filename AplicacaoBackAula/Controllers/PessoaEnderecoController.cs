using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplicacaoBackAula.DTO;
using AplicacaoBackAula.Models;

namespace AplicacaoBackAula.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaEnderecoController : ControllerBase
    {
        private readonly BdAulaBackendContext _context;

        public PessoaEnderecoController(BdAulaBackendContext context)
        {
            _context = context;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<PessoaDTO>>> GetAll()
        {
            var pessoas = await _context.Pessoas
                .Select(p => new PessoaDTO
                {
                    IdPessoa = p.Idpessoa,
                    Nome = p.Nome,
                    cpf = p.Cpf,
                    DataNascimento = p.DataNascimento,  
                    sexo = p.Sexo,
                    email = p.Email, 
                    telefone = p.Telefone,
                    AtivoPessoa = p.AtivoPessoa,
                    Enderecos = p.Enderecos.Select(e => new EnderecoDTO
                    {
                        IdPessoa = e.Idpessoa,
                        CEP = e.Cep,
                        Numero = e.Numero,
                        Complemento = e.Complemento,
                    }).ToList()

                }).ToListAsync();


            return Ok(pessoas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaDTO>> GetPessoa(int id)
        {
            var pessoas = await _context.Pessoas
            .Where(p => p.Idpessoa == id)
           .Select(p => new PessoaDTO
           {
               IdPessoa = p.Idpessoa,
               Nome = p.Nome,
               cpf = p.Cpf,
               DataNascimento = p.DataNascimento,
               sexo = p.Sexo,
               email = p.Email,
               telefone = p.Telefone,
               AtivoPessoa = p.AtivoPessoa,
               Enderecos = p.Enderecos.Select(e => new EnderecoDTO
               {
                   IdPessoa = e.Idpessoa,
                   CEP = e.Cep,
                   Numero = e.Numero,
                   Complemento = e.Complemento,
               }).ToList()

           }).ToListAsync();

            return Ok(pessoas);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaDTO>> Post(PessoaDTO dTO)
        {
            var ObjPessoa = new Pessoa
            {
                Nome = dTO.Nome,
                Cpf = dTO.cpf,
                DataNascimento = dTO.DataNascimento,
                Email = dTO.email,
                Telefone = dTO.telefone,
                AtivoPessoa = dTO.AtivoPessoa,
                Enderecos = dTO.Enderecos.Select(e => new Endereco
                {
                    Cep = e.CEP,
                    Numero = e.Numero,
                    Complemento = e.Complemento,
                }).ToList()

            };
            _context.Pessoas.Add(ObjPessoa);

            await _context.SaveChangesAsync();

            var respostaBd = new PessoaDTO
            {
                IdPessoa = ObjPessoa.Idpessoa,
                Nome = ObjPessoa.Nome,
                cpf = ObjPessoa.Cpf,
                DataNascimento = ObjPessoa.DataNascimento,
                sexo = ObjPessoa.Sexo,
                email = ObjPessoa.Email,
                telefone = ObjPessoa.Telefone,
                Enderecos = ObjPessoa.Enderecos.Select(e => new EnderecoDTO
                {
                    IdPessoa = e.Idpessoa,
                    Numero = e.Numero,
                    CEP = e.Cep,
                    Complemento = e.Complemento
                }).ToList()

            };


            return CreatedAtAction(
                nameof(GetPessoa),
                new { id = ObjPessoa.Idpessoa }, respostaBd);
        }

    }
}
