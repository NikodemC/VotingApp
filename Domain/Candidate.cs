namespace Domain
{
    public class Candidate
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int Votes { get; set; }
    }
}
