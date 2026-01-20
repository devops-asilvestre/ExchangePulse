using Microsoft.AspNetCore.Mvc;
using ExchangePulse.Application.Interfaces;
using ExchangePulse.Application.DTOs;

namespace ExchangePulse.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar moedas (Currency).
    /// Permite listar, cadastrar, atualizar e remover moedas.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _service;

        public CurrencyController(ICurrencyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todas as moedas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _service.GetAllAsync();
            return Ok(currencies);
        }

        /// <summary>
        /// Obtém uma moeda pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currency = await _service.GetByIdAsync(id);
            if (currency == null) return NotFound();
            return Ok(currency);
        }

        /// <summary>
        /// Cadastra uma nova moeda.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurrencyDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza uma moeda existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CurrencyDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Remove uma moeda pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
