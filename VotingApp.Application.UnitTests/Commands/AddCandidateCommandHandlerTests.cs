using Application.Abstractions;
using Application.Candidates.CommandHandlers;
using Application.Candidates.Commands;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Commands
{
    public class AddCandidateCommandHandlerTests
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly AddCandidateCommandHandler _handler;

        public AddCandidateCommandHandlerTests()
        {
            _candidateRepository = Substitute.For<ICandidateRepository>();
            _handler = new AddCandidateCommandHandler(_candidateRepository);
        }

        [Fact]
        public async Task Handle_ShouldAddCandidateToRepository()
        {
            // Arrange
            var addCandidateCommand = new AddCandidate { Name = "John Doe" };
            var expectedCandidate = new Candidate { Name = "John Doe" };

            _candidateRepository
                .AddCandidate(Arg.Any<Candidate>(), Arg.Any<CancellationToken>())
                .Returns(expectedCandidate);

            // Act
            var result = await _handler.Handle(addCandidateCommand, CancellationToken.None);

            // Assert
            await _candidateRepository
                .Received(1)
                .AddCandidate(Arg.Is<Candidate>(c => c.Name == "John Doe"), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldReturnAddedCandidate()
        {
            // Arrange
            var addCandidateCommand = new AddCandidate { Name = "Jane Doe" };
            var expectedCandidate = new Candidate { Name = "Jane Doe" };

            _candidateRepository
                .AddCandidate(Arg.Any<Candidate>(), Arg.Any<CancellationToken>())
                .Returns(expectedCandidate);

            // Act
            var result = await _handler.Handle(addCandidateCommand, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedCandidate);
        }
    }
}
