using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
    }
}
