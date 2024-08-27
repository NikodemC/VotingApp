using Application.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly VotingDbContext _context;

        public CandidateRepository(VotingDbContext context) => _context = context;

        public async Task<ICollection<Candidate>> GetAllCandidates(CancellationToken cancellationToken) => await _context.Candidates.ToListAsync();

        public async Task<Candidate?> GetCandidateById(int id, CancellationToken cancellationToken) => await _context.Candidates.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Candidate> AddCandidate(Candidate candidate, CancellationToken cancellationToken)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate?> IncreaseCandidateVoteCount(int id, CancellationToken cancellationToken)
        {
            var candidateToUpdate = await _context.Candidates.FirstOrDefaultAsync(p => p.Id == id);

            if (candidateToUpdate != null)
            {
                candidateToUpdate.Votes++;
                await _context.SaveChangesAsync();
            }
            return candidateToUpdate;
        }

    }
}
