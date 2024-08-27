using Application.Abstractions;
using Application.Voters.Queries;
using Domain;
using MediatR;

namespace Application.Voters.QueryHandlers
{
    public class GetVoterByIdQueryHandler : IRequestHandler<GetVoterById, Voter?>
    {
        private readonly IVoterRepository _voterRepository;

        public GetVoterByIdQueryHandler(IVoterRepository voterRepository)
            => _voterRepository = voterRepository;

        public async Task<Voter?> Handle(GetVoterById request, CancellationToken cancellationToken)
            => await _voterRepository.GetVoterById(request.Id, cancellationToken);
    }
}
