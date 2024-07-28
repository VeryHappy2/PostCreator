using AutoMapper;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Post.Host.Repositories.Interfaces;
using Post.Host.Services.Interfaces;
using Post.Host.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Post.Host.Data.Entities;
using Post.Host.Data;

namespace Post.UnitTests.Services
{
    public class PostItemServiceTest : ServiceTest<PostItemEntity, PostItemService>
    {
        public PostItemServiceTest() : base()
        {
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
