using Domain;
using MediatR;

namespace Application.Voters.Queries
{
    public class GetAllVoters : IRequest<ICollection<Voter>>
    {
    }
}
