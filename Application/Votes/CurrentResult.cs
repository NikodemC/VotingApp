using Domain;

namespace Application.Votes
{
    public class CurrentResult
    {
        public string? ResponseMessage { get; set; }

        public ICollection<Voter>? Voters { get; set; }

        public ICollection<Candidate>? Candidates { get; set; }
    }
}
