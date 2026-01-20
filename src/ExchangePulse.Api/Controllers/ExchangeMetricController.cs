using ExchangePulse.Application.DTOs;
using ExchangePulse.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangePulse.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar métricas financeiras (ExchangeMetric).
    /// Permite listar, cadastrar, atualizar e remover métricas.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExchangeMetricController : ControllerBase
    {
        private readonly IExchangeMetricService _service;

        public ExchangeMetricController(IExchangeMetricService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todas as métricas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var metrics = await _service.GetAllAsync();
            return Ok(metrics);
        }

        /// <summary>
        /// Obtém uma métrica pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var metric = await _service.GetByIdAsync(id);
            if (metric == null) return NotFound();
            return Ok(metric);
        }

        /// <summary>
        /// Cadastra uma nova métrica.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExchangeMetricDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza uma métrica existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExchangeMetricDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Remove uma métrica pelo ID.
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
