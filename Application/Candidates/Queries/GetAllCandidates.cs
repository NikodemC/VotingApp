using Domain;
using MediatR;

namespace Application.Candidates.Queries
{
    public class GetAllCandidates : IRequest<ICollection<Candidate>>
    {
    }
}
