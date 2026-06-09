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

    /// <summary>
    /// Gets all reports, optionally filtered by status.
    /// </summary>
    /// <param name="status">Optional report status filter. 0 = Diterima, 1 = Diproses, 2 = Selesai, 3 = Ditolak.</param>
    /// <returns>A raw array of reports.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] ReportStatus? status)
    {
        var reports = status.HasValue ? _reportService.GetReportsByStatus(status.Value) : _reportService.GetAllReports();
        return Ok(reports.Select(ReportResponse.FromReport));
    }

    /// <summary>
    /// Gets aggregated report summary for dashboard usage.
    /// </summary>
    /// <returns>Total reports wgrouped by MVP status.</returns>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetSummary() => Ok(_reportService.GetSummary());

    /// <summary>
    /// Tracks a report using report ID and tracking PIN.
    /// </summary>
    /// <param name="request">Report tracking request.</param>
    /// <returns>Public tracking data for the matching report.</returns>
    [HttpPost("track")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Track([FromBody] TrackReportRequest request)
    {
        var trackingData = _reportService.TrackReport(request.ReportId, request.Pin);
        return trackingData == null ? Unauthorized(new { message = "Report ID atau PIN tidak valid." }) : Ok(trackingData);
    }

    /// <summary>
    /// Gets report detail by ID.
    /// </summary>
    /// <param name="id">Report ID.</param>
    /// <returns>The matching report.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var report = _reportService.GetById(id);
        return report == null ? NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." }) : Ok(ReportResponse.FromReport(report));
    }

    /// <summary>
    /// Creates a new report.
    /// </summary>
    /// <param name="request">Report creation request.</param>
    /// <returns>The created report.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return CreatedAtAction(nameof(GetById), new { id = result.ReportId }, ReportResponse.FromReport(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates report data.
    /// </summary>
    /// <param name="id">Report ID.</param>
    /// <param name="request">Report update request.</param>
    /// <returns>The updated report.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UpdateReportRequest request)
    {
        bool updated = _reportService.UpdateReport(id, request.Title, request.Description);
        if (!updated)
        {
            return NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." });
        }

        var report = _reportService.GetById(id);
        return report == null ? NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." }) : Ok(ReportResponse.FromReport(report));
    }

    /// <summary>
    /// Deletes a report.
    /// </summary>
    /// <param name="id">Report ID.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        bool deleted = _reportService.DeleteReport(id);
        return deleted ? NoContent() : NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." });
    }

    /// <summary>
    /// Updates report status.
    /// </summary>
    /// <param name="id">Report ID.</param>
    /// <param name="request">Target status request. 0 = Diterima, 1 = Diproses, 2 = Selesai, 3 = Ditolak.</param>
    /// <returns>The updated report.</returns>
    [HttpPatch("{id}/transition")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Transition(Guid id, [FromBody] TransitionReportStatusRequest request)
    {
        var report = _reportService.GetById(id);
        if (report == null)
        {
            return NotFound(new { message = $"Report dengan ID {id} tidak ditemukan." });
        }

        bool success = _reportService.ExecuteTransition(id, request.Status);
        if (!success)
        {
            return Conflict(new { message = "Invalid status transition." });
        }

        var updatedReport = _reportService.GetById(id);
        return Ok(ReportResponse.FromReport(updatedReport!));
    }

    /// <summary>
    /// Closes a report directly by setting its status to Selesai.
    /// </summary>
    /// <param name="id">Report ID.</param>
    /// <returns>The updated report.</returns>
    [HttpPatch("{id}/close")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Close(Guid id)
    {
        var report = _reportService.CloseReport(id);
        return report == null ? NotFound(new { message = "Report not found." }) : Ok(ReportResponse.FromReport(report));
    }

    /// <summary>
    /// Adds text-based evidence to a report.
    /// </summary>
    /// <param name="id">Report ID.</param>
    /// <param name="request">Text evidence request.</param>
    /// <returns>The created evidence.</returns>
    [HttpPost("{id}/evidences")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
