using Domain;
using MediatR;

namespace Application.Candidates.Commands
{
    public class AddCandidate : IRequest<Candidate>
    {
        public required string Name { get; init; }
    }
}
