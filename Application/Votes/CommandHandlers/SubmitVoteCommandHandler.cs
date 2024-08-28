using Application.Abstractions;
using Application.Votes.Commands;
using MediatR;

namespace Application.Votes.CommandHandlers
{
    public class SubmitVoteCommandHandler : IRequestHandler<SubmitVote, string>
    {
        private readonly IVoterRepository _voterRepository;
        private readonly ICandidateRepository _candidateRepository;

        public SubmitVoteCommandHandler(IVoterRepository voterRepository, ICandidateRepository candidateRepository)
        {
            _voterRepository = voterRepository;
            _candidateRepository = candidateRepository;
        }

        public async Task<string> Handle(SubmitVote request, CancellationToken cancellationToken)
        {
            var voter = await _voterRepository.GetVoterById(request.VoterId, cancellationToken);
            if (voter is null)
            {
                return $"Voter with id {request.VoterId} not found.";
            }

            if (voter.HasVoted)
            {
                return $"Voter with id {request.VoterId} has already voted.";
            }

            var candidate = await _candidateRepository.GetCandidateById(request.CandidateId, cancellationToken);
            if (candidate is null)
            {
                return $"Candidate with id {request.CandidateId} not found.";
            }

            await _voterRepository.MarkAsVoted(request.VoterId, cancellationToken);
            await _candidateRepository.IncreaseCandidateVoteCount(request.CandidateId, cancellationToken);

            return string.Empty;
        }
    }
}
