using Application.Candidates.Queries;
using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using VotingApp.Api.Dtos;
using VotingApp.Api.UnitTests.Helpers;

namespace VotingApp.Api.UnitTests
{
    public class CandidateEndpointTests : IClassFixture<InMemoryDbApplication>
    {
        private readonly InMemoryDbApplication _factory;
        private readonly IMediator _mediator;

        public CandidateEndpointTests(InMemoryDbApplication factory)
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
        public async Task GetCandidateById_ValidId_ReturnsOk()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetCandidateById>()).Returns(new Candidate { Id = 1, Name = "Name" });
            var client = CreateClientWithMediator();

            // Act
            var response = await client.GetAsync("/api/Candidates/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Candidate>();

            result.Should().NotBeNull();
            result.Name.Should().Be("Name");
        }

        [Fact]
        public async Task GetCandidateById_NotValidId_ReturnsNotFound()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetCandidateById>()).Returns((Candidate?)null);
            var client = CreateClientWithMediator();

            // Act
            var response = await client.GetAsync("/api/Candidates/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory, AutoData]
        public async Task GetAllCandidates_ReturnsOk(List<Candidate> Candidates)
        {
            // Arrange
            _mediator.Send(Arg.Any<GetAllCandidates>()).Returns(Candidates);
            var client = CreateClientWithMediator();

            // Act
            var response = await client.GetAsync("/api/Candidates");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateCandidate_ValidCandidateDto_ReturnsCreated()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/Candidates", new CandidateDto("Name"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<Candidate>();

            result.Should().NotBeNull();
            result.Name.Should().Be("Name");
        }
    }
}
