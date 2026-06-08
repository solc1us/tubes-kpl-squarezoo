using Microsoft.AspNetCore.Mvc;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Services;

namespace tubes_kpl_squarezoo.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;
    private readonly UserService _userService;

    public ReportController(ReportService reportService, UserService userService)
    {
        _reportService = reportService;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] ReportStatus? status)
    {
        return Ok(status.HasValue ? _reportService.GetReportsByStatus(status.Value) : _reportService.GetAllReports());
    }

    [HttpGet("summary")]
    public IActionResult GetSummary() => Ok(_reportService.GetSummary());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var report = _reportService.GetById(id);
        return report == null ? NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." }) : Ok(report);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateReportRequest request)
    {
        try
        {
            var user = _userService.GetById(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = $"User dengan ID {request.UserId} tidak ditemukan." });
            }

            var report = new Report(request.Title, request.Description, user);
            var result = _reportService.CreateReport(report);
            return CreatedAtAction(nameof(GetById), new { id = result.ReportId }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdateReportRequest request)
    {
        bool updated = _reportService.UpdateReport(id, request.Title, request.Description);
        if (!updated)
        {
            return NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." });
        }

        return Ok(_reportService.GetById(id));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        bool deleted = _reportService.DeleteReport(id);
        return deleted ? NoContent() : NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." });
    }

    [HttpPatch("{id}/transition")]
    public IActionResult Transition(Guid id, [FromBody] TransitionReportStatusRequest request)
    {
        var report = _reportService.GetById(id);
        if (report == null)
        {
            return NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." });
        }

        bool success = _reportService.ExecuteTransition(id, request.Status);
        return success ? Ok(_reportService.GetById(id)) : BadRequest(new { message = "Status tidak valid." });
    }

    [HttpPatch("{id}/close")]
    public IActionResult Close(Guid id)
    {
        var report = _reportService.CloseReport(id);
        return report == null ? NotFound(new { message = "Report not found." }) : Ok(report);
    }

    [HttpPost("{id}/evidences")]
    public IActionResult AddEvidence(Guid id, [FromBody] AddEvidenceRequest request)
    {
        try
        {
            var evidence = _reportService.AddEvidenceToReport(id, request.Type, request.Content, request.Description);
            return evidence == null ? NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." }) : Ok(evidence);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = $"{ex.ParamName} tidak boleh kosong." });
        }
    }
}
