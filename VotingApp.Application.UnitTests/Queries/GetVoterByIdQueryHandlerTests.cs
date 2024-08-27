using Application.Abstractions;
using Application.Voters.Queries;
using Application.Voters.QueryHandlers;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Queries
{
    public class GetVoterByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnVoterById()
        {
            // Arrange
            var voterRepository = Substitute.For<IVoterRepository>();
            var handler = new GetVoterByIdQueryHandler(voterRepository);

            var getVoterByIdQuery = new GetVoterById { Id = 1 };
            var cancellationToken = CancellationToken.None;

            var expectedVoter = new Voter { Id = 1, Name = "John Doe" };
            voterRepository.GetVoterById(getVoterByIdQuery.Id, cancellationToken).Returns(expectedVoter);

            // Act
            var result = await handler.Handle(getVoterByIdQuery, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedVoter);
        }

        [Fact]
        public async Task Handle_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var voterRepository = Substitute.For<IVoterRepository>();
            var handler = new GetVoterByIdQueryHandler(voterRepository);

            var getVoterByIdQuery = new GetVoterById { Id = 2 };
            var cancellationToken = CancellationToken.None;

            voterRepository.GetVoterById(getVoterByIdQuery.Id, cancellationToken).Returns((Voter?)null);

            // Act
            var result = await handler.Handle(getVoterByIdQuery, cancellationToken);

            // Assert
            result.Should().BeNull();
        }
    }
}
