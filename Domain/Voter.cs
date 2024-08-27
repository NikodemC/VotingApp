namespace Domain
{
    public class Voter
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool HasVoted { get; set; }
    }
}
