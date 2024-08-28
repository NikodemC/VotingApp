using Api.Dtos;
using Application.Votes;
using Application.Votes.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using VotingApp.Api.UnitTests.Helpers;

namespace VotingApp.Api.UnitTests
{
    public class VotesEndpointTests : IClassFixture<InMemoryDbApplication>
    {
        private readonly InMemoryDbApplication _factory;
        private readonly IMediator _mediator;

        public VotesEndpointTests(InMemoryDbApplication factory)
        {
            _factory = factory;
            _mediator = Substitute.For<IMediator>();
        }
        private HttpClient CreateClientWithMediator()
        {
            return _factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_mediator);
            })).CreateClient();
        }

        [Fact]
        public async Task SubmitVote_ValidVoterId_And_ValidCandidateId_ReturnsOk()
        {
            // Arrange
            _mediator.Send(Arg.Any<SubmitVote>()).Returns(string.Empty);
            var client = CreateClientWithMediator();

            // Act
            var response = await client.PostAsJsonAsync("/api/vote/submit", new VoteDto(1,1));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SubmitVote_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mediator.Send(Arg.Any<SubmitVote>()).Returns("Voter with id 1 not found.");
            var client = CreateClientWithMediator();

            // Act
            var response = await client.PostAsJsonAsync("/api/vote/submit", new VoteDto(1, 1));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
