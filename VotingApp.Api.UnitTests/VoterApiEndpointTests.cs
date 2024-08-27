using Application.Voters.Queries;
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
    public class VoterApiEndpointTests : IClassFixture<InMemoryDbApplication>
    {
        private readonly InMemoryDbApplication _factory;
        private readonly IMediator _mediator;

        public VoterApiEndpointTests(InMemoryDbApplication factory)
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
        public async Task GetVoterById_ValidId_ReturnsOk()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetVoterById>()).Returns(new Voter { Id = 1, Name = "Name" });
            var client = CreateClientWithMediator();

            // Act
            var response = await client.GetAsync("/api/Voters/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Voter>();

            result.Should().NotBeNull();
            result.Name.Should().Be("Name");
        }

        [Fact]
        public async Task GetVoterById_NotValidId_ReturnsNotFound()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetVoterById>()).Returns((Voter?)null);
            var client = CreateClientWithMediator();

            // Act
            var response = await client.GetAsync("/api/Voters/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory, AutoData]
        public async Task GetAllVoters_ReturnsOk(List<Voter> Voters)
        {
            // Arrange
            _mediator.Send(Arg.Any<GetAllVoters>()).Returns(Voters);
            var client = CreateClientWithMediator();

            // Act
            var response = await client.GetAsync("/api/Voters");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateVoter_ValidVoterDto_ReturnsCreated()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/Voters", new VoterDto("Name"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<Voter>();

            result.Should().NotBeNull();
            result.Name.Should().Be("Name");
        }
    }
}
