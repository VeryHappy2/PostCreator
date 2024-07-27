using AutoMapper;
using Catalog.Host.Models.Requests;
using FluentAssertions;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Post.Host.Data;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Models.Response;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services;
using Post.Host.Services.Interfaces;

namespace Post.UnitTests.Services
{
    public class PostBffServiceTest
    {
        private readonly IPostBffService _service;

        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<PostBffService>> _logger;
        private readonly Mock<IPostBffRepository> _postBffRepository;
        private readonly Mock<IMapper> _mapper;

        public PostBffServiceTest()
        {
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<PostBffService>>();
            _postBffRepository = new Mock<IPostBffRepository>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();

            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);
            _service = new PostBffService(_dbContextWrapper.Object, _logger.Object, _mapper.Object, _postBffRepository.Object);
        }

        [Fact]
        public async Task GetPostsByUserNameAsync_Success()
        {
            string userName = "Name";
            List<PostItemEntity> postItems = new List<PostItemEntity>
            {
                new PostItemEntity
                {
                    Title = "Title",
                    Id = 14,
                    Content = "Content",
                    Comments = null,
                    CategoryId = 1,
                    Category = new PostCategoryEntity(),
                    Date = DateTime.Now,
                    UserId = "1234",
                    UserName = userName
                },
            };

            List<PostItemDto> postItemsDto = new List<PostItemDto>
            {
                new PostItemDto
                {
                    Title = "Title",
                    Id = 14,
                    Content = "Content",
                    Comments = null,
                    Category = new PostCategoryDto(),
                    Date = DateTime.Now,
                    UserId = "1234",
                    UserName = userName
                },
            };

            _postBffRepository.Setup(x => x.GetPostItemsByUserName(
                It.IsAny<string>())).ReturnsAsync(postItems);

            _mapper.Setup(s => s.Map<List<PostItemDto>>(
                It.Is<List<PostItemEntity>>(i => i.Equals(postItems)))).Returns(postItemsDto);

            var result = await _service.GetPostsByUserNameAsync(userName);

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetPostsByUserNameAsync_Failed()
        {
            string? userName = null;

            _postBffRepository.Setup(x => x.GetPostItemsByUserName(
                It.IsAny<string>())).ReturnsAsync((List<PostItemEntity>?)null);

            var result = await _service.GetPostsByUserNameAsync(userName);

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 4;
            var testTotalCount = 12;
            var search = string.Empty;

            PageItemRequest request = new PageItemRequest
            {
                PageSize = testPageSize,
                CategoryFilter = 1,
                PageIndex = testPageIndex,
                SearchByTitle = search,
                SearchByUserName = string.Empty
            };

            var pagingPaginatedItemsSuccess = new PaginatedItems<PostItemEntity>()
            {
                Data = new List<PostItemEntity>()
                {
                    new PostItemEntity()
                    {
                        Title = "Title",
                        Id = 14,
                        Content = "Content",
                        Comments = null,
                        CategoryId = 1,
                        Category = new PostCategoryEntity(),
                        Date = DateTime.Now,
                        UserId = "1234",
                        UserName = "name",
                    },
                },
                TotalCount = testTotalCount,
            };

            var postItemSuccess = new PostItemEntity()
            {
                Title = "Title",
                Id = 14,
                Content = "Content",
                Comments = null,
                CategoryId = 1,
                Category = new PostCategoryEntity(),
                Date = DateTime.Now,
                UserId = "1234",
                UserName = "name",

            };

            var postItemDtoSuccess = new PostItemDto
            {
                Title = "Title",
                Id = 14,
                Content = "Content",
                Comments = null,
                Category = new PostCategoryDto(),
                Date = DateTime.Now,
                UserId = "1234",
                UserName = "name"
            };

            _postBffRepository.Setup(s => s.GetByPageAsync(
                It.Is<PageItemRequest>(i => i == request)))
                .ReturnsAsync(pagingPaginatedItemsSuccess);

            _mapper.Setup(s => s.Map<PostItemDto>(
                It.Is<PostItemEntity>(i => i.Equals(postItemSuccess)))).Returns(postItemDtoSuccess);

            // act
            var result = await _service.GetPostByPageAsync(request);

            // assert
            result.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Failed()
        {
            // arrange
            var testPageIndex = 10000000;
            var testPageSize = 400000000;
            var testTotalCount = 12123123;
            var search = string.Empty;

            PageItemRequest request = new PageItemRequest
            {
                PageSize = testPageSize,
                CategoryFilter = 1,
                PageIndex = testPageIndex,
                SearchByTitle = search,
                SearchByUserName = string.Empty,
            };

            var pagingPaginatedItemsSuccess = new PaginatedItems<PostItemEntity>()
            {
                Data = new List<PostItemEntity>()
                {
                    new PostItemEntity()
                    {
                        Title = "Title",
                        Id = 14,
                        Content = "Content",
                        Comments = null,
                        CategoryId = 1,
                        Category = new PostCategoryEntity(),
                        Date = DateTime.Now,
                        UserId = "1234",
                        UserName = "name",
                    },
                },
                TotalCount = testTotalCount,
            };

            var postItemSuccess = new PostItemEntity()
            {
                Title = "Title",
                Id = 14,
                Content = "Content",
                Comments = null,
                CategoryId = 1,
                Category = new PostCategoryEntity(),
                Date = DateTime.Now,
                UserId = "1234",
                UserName = "name",

            };

            var postItemDtoSuccess = new PostItemDto
            {
                Title = "Title",
                Id = 14,
                Content = "Content",
                Comments = null,
                Category = new PostCategoryDto(),
                Date = DateTime.Now,
                UserId = "1234",
                UserName = "name"
            };

            _postBffRepository.Setup(s => s.GetByPageAsync(
                It.Is<PageItemRequest>(i => i == request)))
                .ReturnsAsync(pagingPaginatedItemsSuccess);

            // act
            var result = await _service.GetPostByPageAsync(request);

            // assert
            result.Should().Match<PaginatedResponse<PostItemDto>?>(x => x.Data.First() == null);
        }
    }
}
