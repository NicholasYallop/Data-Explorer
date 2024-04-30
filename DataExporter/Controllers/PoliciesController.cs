using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PoliciesController : ControllerBase
	{
		private PolicyService _policyService;

		public PoliciesController(PolicyService policyService)
		{
			_policyService = policyService;
		}

		[HttpPost]
		public async Task<IActionResult> PostPolicies([FromBody] CreatePolicyDto createPolicyDto)
		{
			try
			{
				return Ok(await _policyService.CreatePolicyAsync(createPolicyDto));
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}


		[HttpGet]
		public async Task<IActionResult> GetPolicies()
		{
			try
			{
				var policies = await _policyService.ReadPoliciesAsync();

				if (!policies.Any()) { return NotFound(); }
				return Ok(policies);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

		[HttpGet("{policyId}")]
		public async Task<IActionResult> GetPolicy(int policyId)
		{
			ReadPolicyDto? policy;
			try
			{
				policy = await _policyService.ReadPolicyAsync(policyId);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}

			if (policy is not null)
			{
				return Ok(policy);
			}
			else
			{
				return NotFound();
			}
		}


		[HttpPost("export")]
		public async Task<IActionResult> ExportData([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
		{
			try
			{
				var policies = await _policyService.ExportData(startDate, endDate);

				if (!policies.Any()) { return NotFound(); }
				return Ok(policies);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
