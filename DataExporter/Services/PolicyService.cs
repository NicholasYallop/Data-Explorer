using System.Reflection.Metadata.Ecma335;
using DataExporter.Dtos;
using DataExporter.Model;
using Microsoft.EntityFrameworkCore;


namespace DataExporter.Services
{
    public class PolicyService
    {
        private ExporterDbContext _dbContext;

        public PolicyService(ExporterDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        /// <summary>
        /// Creates a new policy from the DTO.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
        public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
        {
            var policy = new Policy()
            {
               PolicyNumber = createPolicyDto.PolicyNumber,
               Premium = createPolicyDto.Premium,
               StartDate = createPolicyDto.StartDate,
            };

            var addition = await _dbContext.AddAsync(policy);
            _dbContext.SaveChanges();

            return new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };
        }

        /// <summary>
        /// Retrives all policies.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of ReadPoliciesDto.</returns>
        public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync()
        {
            var policies = _dbContext.Policies.Select(policy => new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            });

            return await policies.ToListAsync();
        }

        /// <summary>
        /// Retrieves a policy by id, if that policy exists. Else null.
        /// </summary>
        /// <param name="id">Id of desired policy.</param>
        /// <returns>Returns a ReadPolicyDto when provided id mathces an entry, else null.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id)
        {
            var policy = await _dbContext.Policies.SingleOrDefaultAsync(x => x.Id == id);
            if (policy == null)
            {
                return null;
            }

            var policyDto = new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };

            return policyDto;
        }

        public async Task<IList<ExportDto>> ExportData(DateTime startDate, DateTime endDate)
        {
            var policies = _dbContext.Policies.Where(p => p.StartDate > startDate && p.StartDate < endDate);
            var exports = policies.Select(p => new ExportDto()
            {
                PolicyNumber = p.PolicyNumber,
                Premium = p.Premium,
                StartDate = p.StartDate,
                Notes = p.Notes.Select(n => n.Text).ToList()
            });

            return await exports.ToListAsync();
        }
    }
}
