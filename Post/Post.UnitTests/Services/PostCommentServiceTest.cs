using Post.Host.Services;
using Post.Host.Data.Entities;
using Moq;
using Post.Host.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Infrastructure.Services;
using Post.Host.Data;
using Infrastructure.Services.Interfaces;
using Post.Host.Models.Dtos;

namespace Post.UnitTests.Services
{
    public class PostCommentServiceTest : ServiceTest<PostCommentEntity, PostCommentDto, PostCommentService>
    {
        public PostCommentServiceTest() : base((dbContextWrapper, logger, mapper, baseRepository) =>
            new PostCommentService(
                dbContextWrapper,
                logger,
                mapper,
                baseRepository))
        {
        }

        [Fact]
        public async Task GetByIdAsync_Failed()
        {
            await GetByIdAsync_Failed_Test();
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            await GetByIdAsync_Success_Test();
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            await AddAsync_Success_Test();
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            await AddAsync_Failed_Test();
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            await UpdateAsync_Success_Test();
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            await UpdateAsync_Failed_Test();
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await DeleteAsync_Success_Test();
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            await DeleteAsync_Failed_Test();
        }
    }
}
