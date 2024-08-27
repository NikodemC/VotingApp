using Domain;

namespace Application.Abstractions
{
    public interface IVoterRepository
    {
        Task<Voter> AddVoter(Voter voter, CancellationToken cancellationToken);
        Task<ICollection<Voter>> GetAllVoters(CancellationToken cancellationToken);
        Task<Voter?> GetVoterById(int id, CancellationToken cancellation);
        Task<Voter?> MarkAsVoted(int id, CancellationToken cancellation);
    }
}
