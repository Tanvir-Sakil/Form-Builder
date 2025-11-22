using FormBuilder.Application.Features.Forms.Commands;
using FormBuilder.Application.Features.Forms.Queries;
using FormBuilder.Domain.DTOs;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.Web.Controllers
{
    public class FormController : Controller
    {
        private readonly IMediator _mediator;

        public FormController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CreateFormDto createFormDto)
        {
            if (createFormDto == null || string.IsNullOrWhiteSpace(createFormDto.Title))
            {
                return BadRequest("Invalid Request");
            }
            var createdId = await _mediator.Send(new CreateFormCommand(createFormDto));

            return Json(new { createdId });
        }

        [HttpGet]
        public async Task<IActionResult>Preview(int id)
        {
            var model = await _mediator.Send(new GetFormDetailsQuery(id));

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult>Paged(int start =0,int length =10,string search=null)
        {
            var (total,items)= await _mediator.Send(new GetFormsPagedQuery(start,length,search));

            return Json(new
            {
                recordsTotal = total,
                recordsFiltered = total,
                data = items
            });
        }
    }
}
