using Application.Abstractions;
using Application.Candidates.Queries;
using Domain;
using MediatR;

namespace Application.Candidates.QueryHandlers
{
    public class GetAllCandidatesQueryHandler : IRequestHandler<GetAllCandidates, ICollection<Candidate>>
    {
        private readonly ICandidateRepository _candidateRepository;

        public GetAllCandidatesQueryHandler(ICandidateRepository candidateRepository) 
            => _candidateRepository = candidateRepository;

        public async Task<ICollection<Candidate>> Handle(GetAllCandidates request, CancellationToken cancellationToken) 
            => await _candidateRepository.GetAllCandidates(cancellationToken);
    }
}
