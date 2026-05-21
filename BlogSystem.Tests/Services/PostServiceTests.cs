using Moq;
using AutoMapper;
using BlogSystem.Application.Services;
using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Application.Interfaces.Security;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.Exceptions;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Tests.Services;

public class PostServiceTests
{
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IPostRepository> _postRepo = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<ICurrentUser> _currentUser = new();

    private readonly PostService _service;

    public PostServiceTests()
    {
        _service = new PostService(
            _mapper.Object,
            _postRepo.Object,
            _userRepo.Object,
            _currentUser.Object
        );
    }

    [Fact]
    public async Task Create_ShouldThrow_WhenUserNotAuthenticated()
    {
        _currentUser.Setup(x => x.IsAuthenticated).Returns(false);
        _currentUser.Setup(x => x.UserId).Returns((Guid?)null);

        var dto = new CreatePostDto();

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ShouldThrow_WhenPostNotFound()
    {
        _postRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldThrow_WhenPostNotFound()
    {
        _postRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post)null);

        var dto = new UpdatePostDto();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None));
    }

    [Fact]
    public async Task Create_ShouldThrow_WhenUserNotFound()
    {
        _currentUser.Setup(x => x.IsAuthenticated).Returns(true);
        _currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

        _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        var dto = new CreatePostDto();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.CreateAsync(dto, CancellationToken.None));
    }
}
