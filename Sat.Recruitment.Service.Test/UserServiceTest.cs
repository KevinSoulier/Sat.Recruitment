using Microsoft.EntityFrameworkCore;
using Moq;
using Sat.Recruitment.Model;
using Sat.Recruitment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Xunit;
using Sat.Recruitment.Service;

namespace Sat.Recruitment.Service.Test
{
    public class UserServiceTest
    {
        [Fact]
        public void TestCreateUserPass()
        {
            var data = new List<User>
            {
                new User()
                {
                    Name = "Mike",
                    Email = "mike@gmail.com",
                    Address = "Av. Juan G",
                    Phone = "+349 1122354215",
                    UserType = "Normal",
                    Money = 124
                },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<RepositoryContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);
            
            var result = service.CreateUser(new User()
            {
                Name = "Kevin",
                Email = "Kevin@gmail.com",
                Address = "Av. aaa G",
                Phone = "+349 123345674",
                UserType = "SuperUser",
                Money = 200
            });

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Message);           

        }

        [Fact]
        public void TestCreateUserDuplicated()
        {
            var data = new List<User>
            {
                new User()
                {
                    Name = "Mike",
                    Email = "mike@gmail.com",
                    Address = "Av. Juan G",
                    Phone = "+349 1122354215",
                    UserType = "Normal",
                    Money = 124
                },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<RepositoryContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);

            var result = service.CreateUser(new User()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            });

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Message);

        }

    }
}
