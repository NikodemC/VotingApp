using Domain;

namespace Application.Abstractions
{
    public interface ICandidateRepository
    {
        Task<Candidate> AddCandidate(Candidate candidate, CancellationToken cancellationToken);
        Task<ICollection<Candidate>> GetAllCandidates(CancellationToken cancellationToken);
        Task<Candidate?> GetCandidateById(int id, CancellationToken cancellationToken);
        Task<Candidate?> IncreaseCandidateVoteCount(int id, CancellationToken cancellationToken);
    }
}
