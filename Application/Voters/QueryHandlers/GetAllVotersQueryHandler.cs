using Application.Abstractions;
using Application.Voters.Queries;
using Domain;
using MediatR;

namespace Application.Voters.QueryHandlers
{
    public class GetAllVotersQueryHandler : IRequestHandler<GetAllVoters, ICollection<Voter>>
    {
        private readonly IVoterRepository _voterRepository;

        public GetAllVotersQueryHandler(IVoterRepository voterRepository)
            => _voterRepository = voterRepository;

        public async Task<ICollection<Voter>> Handle(GetAllVoters request, CancellationToken cancellationToken)
            => await _voterRepository.GetAllVoters(cancellationToken);
    }
}
