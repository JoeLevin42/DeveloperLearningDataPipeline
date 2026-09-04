using Microsoft.AspNetCore.Mvc;
using SurveyApi.Models;
using SurveyApi.Services;

namespace SurveyApi.Controllers;

[ApiController]
[Route("[controller]")]

public class SurveryController : ControllerBase
{
    private readonly SurveyService _service;

    public SurveryController(SurveyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SurveyModel>>> GellAllSurveyAsync()
    {
        var all = await _service.GetAllAsync();
        return Ok(all);
    }

    [HttpGet("UseDocs")]
    public async Task<ActionResult<IEnumerable<SurveyModel>>> GetWhoUseDocs()
    {
        var res = await _service.GetWhoUseDocs();

        return Ok(res);
    }
    [HttpGet("GetAITrust")]
    public async Task<ActionResult<IEnumerable<SurveyModel>>> GetTrustAi()
    {
        var res = await _service.GetTrustAi();

        return Ok(res);
    }
    [HttpGet("GetTop10")]
    public async Task<ActionResult<IEnumerable<SurveyModel>>> GetSeniorUp10()
    {
        var res = await _service.GetSeniorUp10();

        return Ok(res);
    }

    [HttpGet("Get20")]
    public async Task<ActionResult<IEnumerable<SurveyModel>>> Get20Async()
    {
        var result = await _service.Get20Async();
        return Ok(result);
    }


 }