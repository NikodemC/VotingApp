using Application.Abstractions;
using Application.Votes.Commands;
using MediatR;

namespace Application.Votes.CommandHandlers
{
    public class SubmitVoteCommandHandler : IRequestHandler<SubmitVote, CurrentResult>
    {
        private readonly IVoterRepository _voterRepository;
        private readonly ICandidateRepository _candidateRepository;

        public SubmitVoteCommandHandler(IVoterRepository voterRepository, ICandidateRepository candidateRepository)
        {
            _voterRepository = voterRepository;
            _candidateRepository = candidateRepository;
        }

        public async Task<CurrentResult> Handle(SubmitVote request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var voter = await _voterRepository.GetVoterById(request.VoterId, cancellationToken);
            if (voter is null)
            {
                return CreateErrorResponse($"Voter with id {request.VoterId} not found.");
            }

            if (voter.HasVoted)
            {
                return CreateErrorResponse($"Voter with id {request.VoterId} has already voted.");
            }

            var candidate = await _candidateRepository.GetCandidateById(request.CandidateId, cancellationToken);
            if (candidate is null)
            {
                return CreateErrorResponse($"Candidate with id {request.CandidateId} not found.");
            }

            await _voterRepository.MarkAsVoted(request.VoterId, cancellationToken);
            await _candidateRepository.IncreaseCandidateVoteCount(request.CandidateId, cancellationToken);

            return await CreateCurrentResult(cancellationToken);
        }

        private static CurrentResult CreateErrorResponse(string message) => new CurrentResult { ResponseMessage = message };

        private async Task<CurrentResult> CreateCurrentResult(CancellationToken cancellationToken) 
            => new CurrentResult
            {
                Voters = await _voterRepository.GetAllVoters(cancellationToken),
                Candidates = await _candidateRepository.GetAllCandidates(cancellationToken)
            };
    }
}
