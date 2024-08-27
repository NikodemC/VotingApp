using Application.Abstractions;
using Application.Candidates.Commands;
using Domain;
using MediatR;

namespace Application.Candidates.CommandHandlers
{
    public class AddCandidateCommandHandler : IRequestHandler<AddCandidate, Candidate>
    {
        private readonly ICandidateRepository _candidateRepository;

        public AddCandidateCommandHandler(ICandidateRepository candidateRepository)
            => _candidateRepository = candidateRepository;

        public async Task<Candidate> Handle(AddCandidate request, CancellationToken cancellationToken)
        {
            var candidate = new Candidate() { Name = request.Name };
            return await _candidateRepository.AddCandidate(candidate, cancellationToken);
        }
    }
}
