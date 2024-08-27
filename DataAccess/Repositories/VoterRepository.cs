using Application.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class VoterRepository : IVoterRepository
    {
        private readonly VotingDbContext _context;

        public VoterRepository(VotingDbContext context) => _context = context;

        public async Task<ICollection<Voter>> GetAllVoters(CancellationToken cancellationToken) => await _context.Voters.ToListAsync();

        public async Task<Voter?> GetVoterById(int id, CancellationToken cancellation) => await _context.Voters.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Voter> AddVoter(Voter voter, CancellationToken cancellationToken)
        {
            _context.Voters.Add(voter);
            await _context.SaveChangesAsync();
            return voter;
        }

        public async Task<Voter?> MarkAsVoted(int id, CancellationToken cancellation)
        {
            var voterToUpdate = await _context.Voters.FirstOrDefaultAsync(p => p.Id == id);

            if (voterToUpdate != null)
            {
                voterToUpdate.HasVoted = true;
                await _context.SaveChangesAsync();
            }
            return voterToUpdate;
        }
    }
}
