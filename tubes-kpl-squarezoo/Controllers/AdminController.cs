using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace tubes_kpl_squarezoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ReportService _service;

        // Constructor untuk menggunakan ReportService melalui dependency injection
        public AdminController(ReportService service)
        {
            _service = service;
        }

        // API untuk mengambil seluruh data report
        [HttpGet("reports")]
        public ActionResult<List<Report>> GetAllReports()
        {
            return Ok(_service.GetAllReports());
        }

        // API untuk mengambil report berdasarkan status
        [HttpGet("reports/status/{status}")]
        public ActionResult<List<Report>> GetByStatus(ReportStatus status)
        {
            var reports = _service.GetAllReports()
                .Where(report => report.Status == status)
                .ToList();

            return Ok(reports);
        }

        // API untuk membuat summary jumlah report berdasarkan status
        [HttpGet("reports/summary")]
        public ActionResult<Dictionary<ReportStatus, int>> GetSummary()
        {
            var summary = _service.GetAllReports()
                .GroupBy(report => report.Status)
                .ToDictionary(group => group.Key, group => group.Count());

            return Ok(summary);
        }

        // API untuk menutup report
        [HttpPut("reports/{reportId}/close")]
        public ActionResult CloseReport(Guid reportId)
        {
            Report? report = _service.GetById(reportId);

            // Return jika report tidak ditemukan
            if (report == null)
            {
                return NotFound("Report tidak ditemukan");
            }

            // Validasi transisi status report
            bool success = report.TransitionTo(ReportStatus.Closed);

            // Return jika report belum bisa ditutup
            if (!success)
            {
                return BadRequest("Report belum bisa ditutup karena status belum valid");
            }

            // Simpan perubahan ke file JSON
            _service.SaveToFile();

            return Ok("Report berhasil ditutup");
        }
    }
}