using Domain;
using MediatR;

namespace Application.Voters.Commands
{
    public class AddVoter : IRequest<Voter>
    {
        public required string Name { get; init; }
    }
}
