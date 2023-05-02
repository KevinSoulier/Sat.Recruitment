using System;
using System.Dynamic;

using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Model;
using Sat.Recruitment.Service;
using Sat.Recruitment.Service.Model;
using Xunit;

namespace Sat.Recruitment.Api.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerTest
    {
        [Fact]
        public async void TestUserCreatedPass()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(p => p.CreateUser(It.IsAny<User>())).Returns(new Result() { IsSuccess = true, Message = "User Created"});

            var mockUser = new Mock<UserRequest>();

            var userController = new UsersController(mockService.Object);
            var actionResult = await  userController.Create(mockUser.Object);

            var result = (actionResult.Result as OkObjectResult).Value as Result;
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Message);
        }

        [Fact]
        public async void TestUserCreatedFail()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(p => p.CreateUser(It.IsAny<User>())).Returns(new Result() { IsSuccess = false, Message = "The user is duplicated" });

            var mockUser = new Mock<UserRequest>();

            var userController = new UsersController(mockService.Object);
            var actionResult = await userController.Create(mockUser.Object);

            var result = (actionResult.Result as BadRequestObjectResult).Value as Result;
            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Message);
        }
    }
}
