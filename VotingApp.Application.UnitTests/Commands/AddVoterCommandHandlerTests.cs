using Application.Abstractions;
using Application.Voters.CommandHandlers;
using Application.Voters.Commands;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Commands
{
    public class AddVoterCommandHandlerTests
    {
        private readonly IVoterRepository _voterRepository;
        private readonly AddVoterCommandHandler _handler;

        public AddVoterCommandHandlerTests()
        {
            _voterRepository = Substitute.For<IVoterRepository>();
            _handler = new AddVoterCommandHandler(_voterRepository);
        }

        [Fact]
        public async Task Handle_ShouldAddVoterToRepository()
        {
            // Arrange
            var addVoterCommand = new AddVoter { Name = "John Doe" };
            var expectedVoter = new Voter { Name = "John Doe" };

            _voterRepository
                .AddVoter(Arg.Any<Voter>(), Arg.Any<CancellationToken>())
                .Returns(expectedVoter);

            // Act
            var result = await _handler.Handle(addVoterCommand, CancellationToken.None);

            // Assert
            await _voterRepository
                .Received(1)
                .AddVoter(Arg.Is<Voter>(c => c.Name == "John Doe"), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldReturnAddedVoter()
        {
            // Arrange
            var addVoterCommand = new AddVoter { Name = "Jane Doe" };
            var expectedVoter = new Voter { Name = "Jane Doe" };

            _voterRepository
                .AddVoter(Arg.Any<Voter>(), Arg.Any<CancellationToken>())
                .Returns(expectedVoter);

            // Act
            var result = await _handler.Handle(addVoterCommand, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedVoter);
        }
    }
}
