using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common; 
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetAll()
        {
            var sales = await _saleService.GetSalesAsync();
            return sales.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetById(Guid id)
        {
            try
            {
                var sale = await _saleService.GetSaleByIdAsync(id);
                return sale;
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Sale sale)
        {
            try
            {
                var createdSale = await _saleService.CreateSaleAsync(sale);
                return CreatedAtAction(nameof(GetById), new { id = createdSale.Id }, createdSale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Sale sale)
        {
            try
            {
                await _saleService.UpdateSaleAsync(id, sale);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelSale(Guid id)
        {
            try
            {
                await _saleService.CancelSaleAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}