using Application.Abstractions;
using AutoFixture.Xunit2;
using DataAccess;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.DataAccess.UnitTests
{
    public class VoterRepositoryTests
    {
        private readonly IVoterRepository _voterRepository;
        private readonly CancellationToken _ct = CancellationToken.None;

        public VoterRepositoryTests()
        {
            DbContextOptionsBuilder<VotingDbContext> options = new();
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _voterRepository = new VoterRepository(new VotingDbContext(options.Options));
        }

        [Theory, AutoData]
        public async Task AddVoter_ShouldPopulateId(Voter voter)
        {
            await _voterRepository.AddVoter(voter, _ct);

            voter.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task GetAllVoters_ShouldReturnListOfVoters()
        {
            // Arrange
            var Voters = new List<Voter>
            {
                new() { Id = 1, Name = "Voter1" },
                new() { Id = 2, Name = "Voter2" },
            };

            await _voterRepository.AddVoter(Voters[0], _ct);
            await _voterRepository.AddVoter(Voters[1], _ct);

            // Act
            var result = await _voterRepository.GetAllVoters(_ct);

            // Assert
            result.Should().BeEquivalentTo(Voters);
        }

        [Fact]
        public async Task GetVoterById_WithValidId_ShouldReturnVoter()
        {
            // Arrange
            var VoterId = 1;
            var Voter = new Voter { Id = VoterId, Name = "Voter1" };

            await _voterRepository.AddVoter(Voter, _ct);

            // Act
            var result = await _voterRepository.GetVoterById(VoterId, _ct);

            // Assert
            result.Should().BeEquivalentTo(Voter);
        }

        [Fact]
        public async Task MarkAsVoted_WithValidVoter_ShouldChangeFlag()
        {
            // Arrange
            var voter = new Voter { Id = 1, Name = "OriginalVoter", HasVoted = false };
            await _voterRepository.AddVoter(voter, _ct);

            // Act
            await _voterRepository.MarkAsVoted(voter.Id, _ct);

            // Assert
            var result = await _voterRepository.GetVoterById(1, _ct);
            result.Name.Should().BeEquivalentTo(voter.Name);
            result.HasVoted.Should().Be(true);
        }
    }
}
