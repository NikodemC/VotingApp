using Application.Abstractions;
using Application.Candidates.Queries;
using Domain;
using MediatR;

namespace Application.Candidates.QueryHandlers
{
    public class GetCandidateByIdQueryHandler : IRequestHandler<GetCandidateById, Candidate?>
    {
        private readonly ICandidateRepository _candidateRepository;

        public GetCandidateByIdQueryHandler(ICandidateRepository candidateRepository)
            => _candidateRepository = candidateRepository;

        public async Task<Candidate?> Handle(GetCandidateById request, CancellationToken cancellationToken)
            => await _candidateRepository.GetCandidateById(request.Id, cancellationToken);
    }
}
