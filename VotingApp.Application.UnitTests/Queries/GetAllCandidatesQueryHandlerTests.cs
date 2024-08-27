using Application.Abstractions;
using Application.Candidates.Queries;
using Application.Candidates.QueryHandlers;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Queries
{
    public class GetAllCandidatesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllCandidates()
        {
            // Arrange
            var candidateRepository = Substitute.For<ICandidateRepository>();
            var handler = new GetAllCandidatesQueryHandler(candidateRepository);

            var getAllCandidatesQuery = new GetAllCandidates();
            var cancellationToken = CancellationToken.None;

            var expectedCandidates = new List<Candidate>
            {
                new() { Id = 1, Name = "Candidate1" },
                new() { Id = 2, Name = "Candidate2" }
            };

            candidateRepository.GetAllCandidates(cancellationToken).Returns(expectedCandidates);

            // Act
            var result = await handler.Handle(getAllCandidatesQuery, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedCandidates);
        }
    }
}
