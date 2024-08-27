using Domain;
using MediatR;

namespace Application.Candidates.Queries
{
    public class GetCandidateById : IRequest<Candidate?>
    {
        public int Id { get; init; }
    }
}
