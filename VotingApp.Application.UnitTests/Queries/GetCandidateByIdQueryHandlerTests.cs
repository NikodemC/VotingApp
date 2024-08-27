using Application.Abstractions;
using Application.Candidates.Queries;
using Application.Candidates.QueryHandlers;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Queries
{
    public class GetCandidateByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnCandidateById()
        {
            // Arrange
            var candidateRepository = Substitute.For<ICandidateRepository>();
            var handler = new GetCandidateByIdQueryHandler(candidateRepository);

            var getCandidateByIdQuery = new GetCandidateById { Id = 1 };
            var cancellationToken = CancellationToken.None;

            var expectedCandidate = new Candidate { Id = 1, Name = "John Doe"};
            candidateRepository.GetCandidateById(getCandidateByIdQuery.Id, cancellationToken).Returns(expectedCandidate);

            // Act
            var result = await handler.Handle(getCandidateByIdQuery, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedCandidate);
        }

        [Fact]
        public async Task Handle_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var candidateRepository = Substitute.For<ICandidateRepository>();
            var handler = new GetCandidateByIdQueryHandler(candidateRepository);

            var getCandidateByIdQuery = new GetCandidateById { Id = 2 };
            var cancellationToken = CancellationToken.None;

            candidateRepository.GetCandidateById(getCandidateByIdQuery.Id, cancellationToken).Returns((Candidate?)null);

            // Act
            var result = await handler.Handle(getCandidateByIdQuery, cancellationToken);

            // Assert
            result.Should().BeNull();
        }
    }
}
