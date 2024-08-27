using Domain;
using MediatR;

namespace Application.Voters.Queries
{
    public class GetVoterById : IRequest<Voter?>
    {
        public int Id { get; init; }
    }
}
