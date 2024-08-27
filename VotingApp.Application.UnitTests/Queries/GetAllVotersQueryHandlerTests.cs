using Application.Abstractions;
using Application.Voters.Queries;
using Application.Voters.QueryHandlers;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Queries
{
    public class GetAllVotersQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllVoters()
        {
            // Arrange
            var voterRepository = Substitute.For<IVoterRepository>();
            var handler = new GetAllVotersQueryHandler(voterRepository);

            var getAllVotersQuery = new GetAllVoters();
            var cancellationToken = CancellationToken.None;

            var expectedVoters = new List<Voter>
            {
                new() { Id = 1, Name = "Voter1" },
                new() { Id = 2, Name = "Voter2" }
            };

            voterRepository.GetAllVoters(cancellationToken).Returns(expectedVoters);

            // Act
            var result = await handler.Handle(getAllVotersQuery, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedVoters);
        }
    }
}
