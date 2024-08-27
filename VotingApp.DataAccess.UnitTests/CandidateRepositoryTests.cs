using Application.Abstractions;
using AutoFixture.Xunit2;
using DataAccess;
using DataAccess.Repositories;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.DataAccess.UnitTests
{
    public class CandidateRepositoryTests
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly CancellationToken _ct = CancellationToken.None;

        public CandidateRepositoryTests()
        {
            DbContextOptionsBuilder<VotingDbContext> options = new();
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _candidateRepository = new CandidateRepository(new VotingDbContext(options.Options));
        }

        [Theory, AutoData]
        public async Task AddCandidate_ShouldPopulateId(Candidate candidate)
        {
            await _candidateRepository.AddCandidate(candidate, _ct);

            candidate.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task GetAllCandidates_ShouldReturnListOfCandidates()
        {
            // Arrange
            var Candidates = new List<Candidate>
            {
                new() { Id = 1, Name = "Candidate1" },
                new() { Id = 2, Name = "Candidate2" },
            };

            await _candidateRepository.AddCandidate(Candidates[0], _ct);
            await _candidateRepository.AddCandidate(Candidates[1], _ct);

            // Act
            var result = await _candidateRepository.GetAllCandidates(_ct);

            // Assert
            result.Should().BeEquivalentTo(Candidates);
        }

        [Fact]
        public async Task GetCandidateById_WithValidId_ShouldReturnCandidate()
        {
            // Arrange
            var CandidateId = 1;
            var Candidate = new Candidate { Id = CandidateId, Name = "Candidate1" };

            await _candidateRepository.AddCandidate(Candidate, _ct);

            // Act
            var result = await _candidateRepository.GetCandidateById(CandidateId, _ct);

            // Assert
            result.Should().BeEquivalentTo(Candidate);
        }

        [Fact]
        public async Task IncreaseCandidateVoteCount_WithValidCandidate_ShouldUpdateVoteCount()
        {
            // Arrange
            var candidate = new Candidate { Id = 1, Name = "OriginalCandidate", Votes = 0 };
            await _candidateRepository.AddCandidate(candidate, _ct);

            // Act
            await _candidateRepository.IncreaseCandidateVoteCount(candidate.Id, _ct);

            // Assert
            var result = await _candidateRepository.GetCandidateById(1, _ct);
            result.Name.Should().BeEquivalentTo(candidate.Name);
            result.Votes.Should().Be(1);
        }
    }
}
