using MediatR;

namespace Application.Votes.Commands
{
    public class SubmitVote : IRequest<CurrentResult>
    {
        public int VoterId { get; init; }

        public int CandidateId { get; set; }
    }
}
