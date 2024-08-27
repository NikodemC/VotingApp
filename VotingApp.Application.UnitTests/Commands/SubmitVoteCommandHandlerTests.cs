using Application.Abstractions;
using Application.Votes.CommandHandlers;
using Application.Votes.Commands;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace VotingApp.Application.UnitTests.Commands
{
    public class SubmitVoteCommandHandlerTests
    {
        private readonly IVoterRepository _voterRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly SubmitVoteCommandHandler _handler;

        public SubmitVoteCommandHandlerTests()
        {
            _voterRepository = Substitute.For<IVoterRepository>();
            _candidateRepository = Substitute.For<ICandidateRepository>();
            _handler = new SubmitVoteCommandHandler(_voterRepository, _candidateRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenVoterNotFound()
        {
            // Arrange
            var submitVoteCommand = new SubmitVote { VoterId = 1, CandidateId = 1 };

            _voterRepository.GetVoterById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Voter)null);

            // Act
            var result = await _handler.Handle(submitVoteCommand, CancellationToken.None);

            // Assert
            result.ResponseMessage.Should().Be("Voter with id 1 not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenVoterHasAlreadyVoted()
        {
            // Arrange
            var submitVoteCommand = new SubmitVote { VoterId = 1, CandidateId = 1 };
            var existingVoter = new Voter { Name = "John Doe", HasVoted = true };

            _voterRepository.GetVoterById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(existingVoter);

            // Act
            var result = await _handler.Handle(submitVoteCommand, CancellationToken.None);

            // Assert
            result.ResponseMessage.Should().Be("Voter with id 1 has already voted.");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenCandidateNotFound()
        {
            // Arrange
            var submitVoteCommand = new SubmitVote { VoterId = 1, CandidateId = 1 };
            var existingVoter = new Voter { Name = "John Doe", HasVoted = false };

            _voterRepository.GetVoterById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(existingVoter);
            _candidateRepository.GetCandidateById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Candidate)null);

            // Act
            var result = await _handler.Handle(submitVoteCommand, CancellationToken.None);

            // Assert
            result.ResponseMessage.Should().Be("Candidate with id 1 not found.");
        }

        [Fact]
        public async Task Handle_ShouldUpdateVoterAndCandidateAndReturnCurrentResult()
        {
            // Arrange
            var submitVoteCommand = new SubmitVote { VoterId = 1, CandidateId = 1 };
            var existingVoter = new Voter { Name = "John Doe", HasVoted = false };
            var existingCandidate = new Candidate { Id = 1, Name = "John Doe", Votes = 0 };
            var allVoters = new List<Voter> { existingVoter };
            var allCandidates = new List<Candidate> { existingCandidate };

            _voterRepository.GetVoterById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(existingVoter);
            _candidateRepository.GetCandidateById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(existingCandidate);
            _voterRepository.GetAllVoters(Arg.Any<CancellationToken>()).Returns(allVoters);
            _candidateRepository.GetAllCandidates(Arg.Any<CancellationToken>()).Returns(allCandidates);

            // Act
            var result = await _handler.Handle(submitVoteCommand, CancellationToken.None);

            // Assert
            await _voterRepository.Received(1).MarkAsVoted(submitVoteCommand.VoterId, Arg.Any<CancellationToken>());
            await _candidateRepository.Received(1).IncreaseCandidateVoteCount(submitVoteCommand.CandidateId, Arg.Any<CancellationToken>());

            result.Voters.Should().BeEquivalentTo(allVoters);
            result.Candidates.Should().BeEquivalentTo(allCandidates);
            result.ResponseMessage.Should().BeNull();
        }
    }
}
