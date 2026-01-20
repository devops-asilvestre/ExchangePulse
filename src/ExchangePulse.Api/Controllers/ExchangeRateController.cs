using ExchangePulse.Application.DTOs;
using ExchangePulse.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangePulse.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar cotações (ExchangeRate).
    /// Permite listar, cadastrar, atualizar e remover cotações.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _service;

        public ExchangeRateController(IExchangeRateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todas as cotações cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rates = await _service.GetAllAsync();
            return Ok(rates);
        }

        /// <summary>
        /// Obtém uma cotação pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rate = await _service.GetByIdAsync(id);
            if (rate == null) return NotFound();
            return Ok(rate);
        }

        /// <summary>
        /// Obtém histórico de cotações de uma moeda em um período.
        /// </summary>
        [HttpGet("currency/{currencyId}")]
        public async Task<IActionResult> GetByCurrency(Guid currencyId, DateTime start, DateTime end)
        {
            var history = await _service.GetByCurrencyAsync(currencyId, start, end);
            return Ok(history);
        }

        /// <summary>
        /// Cadastra uma nova cotação.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExchangeRateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza uma cotação existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExchangeRateDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Remove uma cotação pelo ID.
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
