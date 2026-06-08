using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    public IActionResult Create([FromBody] CreateReportRequest request)
    {
        try
        {
            // Cek user dulu, kalau belum ada lempar error
            var user = _userService.GetById(request.UserId);
                
            if (user == null)
            {
                return NotFound($"User dengan ID {request.UserId} tidak ditemukan.");
            }

            var report = new Report(request.Title, request.Description, user);
            var result = _reportService.CreateReport(report);
            return CreatedAtAction(nameof(GetById), new { id = result.ReportId }, result);
        }
        catch (InvalidOperationException ex)
        {
            // Menangkap Contract Violation (Pre-condition)
            return BadRequest(ex.Message);
        }
    }

    // Endpoint untuk update report itu bisa update title dan description
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdateReportRequest request)
    {
        try
        {
            var updatedReport = _reportService.UpdateReport(id, request.Title, request.Description);

            // return isi dari report yang sudah diupdate, kalau id nya ga ketemu bakal dilempar exception dan ditangkap di catch block
            var report = _reportService.GetById(id);

            return report == null ? NotFound($"Report dengan ID {id} tidak ditemukan.") : Ok(report);
            
        }
        catch (KeyNotFoundException ex)
        {
            // Menangkap Contract Violation (Pre-condition)
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_reportService.GetAllReports());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var report = _reportService.GetById(id);
        return report == null ? NotFound($"Report dengan ID {id} tidak ditemukan.") : Ok(report);
    }

    [HttpPatch("{id}/transition")]
    public IActionResult Transition(Guid id, [FromBody] TransitionRequest request)
    {
        // Cek dulu apakah ada report dengan id dari user
        var report = _reportService.GetById(id);
        if (report == null)
        {
            return NotFound($"Report dengan ID {id} tidak ditemukan.");
        }

        // ExecuteTransition sudah punya logic Automata + Post-condition
        bool success = _reportService.ExecuteTransition(id, request.NextStatus);

        if (!success)
            return BadRequest("Transisi status tidak valid menurut aturan Automata.");

        return Ok(_reportService.GetById(id));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var report = _reportService.GetById(id);
        if (report == null)
        {
            return NotFound($"Report dengan ID {id} tidak ditemukan.");
        }

        bool deleted = _reportService.DeleteReport(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id}/evidences")]
    public IActionResult AddEvidence(Guid id, [FromBody] AddEvidenceRequest request)
    {
        // EvidenceType itu Enum (Image, Video, Text, dll)
        var evidence = _reportService.AddEvidenceToReport(id, request.Type, request.Content, request.Description);
        return evidence == null ? NotFound($"Report dengan id {id} tidak ditemukan.") : Ok(evidence);
    }

}
