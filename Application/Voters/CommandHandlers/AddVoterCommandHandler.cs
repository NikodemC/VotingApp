using Application.Abstractions;
using Application.Voters.Commands;
using Domain;
using MediatR;

namespace Application.Voters.CommandHandlers
{
    public class AddVoterCommandHandler : IRequestHandler<AddVoter, Voter>
    {
        private readonly IVoterRepository _voterRepository;

        public AddVoterCommandHandler(IVoterRepository voterRepository)
            => _voterRepository = voterRepository;

        public async Task<Voter> Handle(AddVoter request, CancellationToken cancellationToken)
        {
            var voter = new Voter() { Name = request.Name };
            return await _voterRepository.AddVoter(voter, cancellationToken);
        }
    }
}
