using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XunitAssessment.Controllers;
using XunitAssessment.Models;
using XunitAssessment.Services.Interfaces;

namespace Tests.Controllers
{
    public class AoColumnControllerTests
    {

        private readonly IFixture fixture;
        private readonly Mock<AocolumnInterface> columnInterface;
        private readonly AoColumnController columnController;

        public AoColumnControllerTests()
        {
            fixture = new Fixture();
            fixture.Customize<Aocolumn>(composer => composer.Without(t => t.Table));
            columnInterface = fixture.Freeze<Mock<AocolumnInterface>>();
            columnController = new AoColumnController(columnInterface.Object);
        }


        //Add a column for the AOTable record created in the above  API
        

        [Fact]
        public async Task AddColumn_ShouldReturnOk_WhenSuccess()
        {
            // Arrange
            var column = fixture.Create<Aocolumn>();
            var returnData = fixture.Create<Aocolumn>();
            columnInterface.Setup(c => c.AddColumn(column)).ReturnsAsync(returnData);

            // Act
            var result = await columnController.AddColumn(column) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            var addedColumn = result.Value as Aocolumn;
            addedColumn.Should().NotBeNull();

            addedColumn.Should().BeEquivalentTo(returnData); // This checks that the added column is equivalent to the return data

            columnInterface.Verify(t => t.AddColumn(column), Times.Once());
        }



        [Fact]
        public void AddColumn_ShouldReturnBadRequest_WhenForeignkeyIsInValid()
        {
            //Arrange
            var column = fixture.Create<Aocolumn>();
            column.TableId = null;
            var expectedExceptionMessage = "Please give a valid foreign key";
            columnInterface.Setup(c => c.AddColumn(column)).Throws(new Exception(expectedExceptionMessage));

            //Act
            var result = columnController.AddColumn(column);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            columnInterface.Verify(t => t.AddColumn(column), Times.Once());
        }

        [Fact]
        public void AddColumn_ShouldReturnBadRequest_WhenInputObjectIsNull()
        {
            //Arrange
            Aocolumn column = null;
            columnInterface.Setup(c => c.AddColumn(column)).ReturnsAsync((Aocolumn)null);

            //Act
            var result = columnController.AddColumn(column);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            columnInterface.Verify(t => t.AddColumn(column), Times.Never());

        }

        [Fact]
        public void AddColumn_ShouldReturnNotFound_WhenAddFailed()
        {
            //Arrange
            var column = fixture.Create<Aocolumn>();
            columnInterface.Setup(c => c.AddColumn(column)).Returns(Task.FromResult<Aocolumn>(null));

            //Act
            var result = columnController.AddColumn(column);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            columnInterface.Verify(t => t.AddColumn(column), Times.Once());
        }









        //Delete a record
        [Fact]
        public void DeleteColumn_ShouldReturnOk_WhenSuccess()
        {
            //Arrange
            Guid Id = fixture.Create<Guid>();
            var returnData = fixture.Create<Aocolumn>();
            columnInterface.Setup(c => c.DeleteColumn(Id)).ReturnsAsync(returnData);

            //Act
            var result = columnController.DeleteColumn(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            columnInterface.Verify(t => t.DeleteColumn(Id), Times.Once());
        }

       


        [Fact]
        public async Task DeleteColumn_ShouldReturnNotFound_WhenFailed()
        {
            // Arrange
            Guid Id = fixture.Create<Guid>();
            columnInterface.Setup(c => c.DeleteColumn(Id)).Returns(Task.FromResult<Aocolumn>(null));

            // Act
            var result = await columnController.DeleteColumn(Id) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("Cannot delete because not found");

            columnInterface.Verify(t => t.DeleteColumn(Id), Times.Once());
        }


        
        [Fact]
        public async Task DeleteColumn_ShouldReturnBadRequestObjectResult_WhenAnExceptionOccurred()
        {
            // Arrange
            Guid Id = fixture.Create<Guid>();
            var exceptionMessage = "ExpectedExceptionMessage";
            columnInterface.Setup(c => c.DeleteColumn(Id)).Throws(new Exception(exceptionMessage));

            // Act
            var result = await columnController.DeleteColumn(Id) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var actualExceptionMessage = result.Value as string;
            actualExceptionMessage.Should().NotBeNull();
            actualExceptionMessage.Should().Be(exceptionMessage);

            columnInterface.Verify(t => t.DeleteColumn(Id), Times.Once());
        }



        //Get all records with DataType "int" and "uniqueidentifier" for a particular Table by passing TableName(AOTable) as parameter
        [Fact]
        public void GetColumnsByType_ShouldReturnOk_WhenSuccess()
        {
            //Arrange
            var name = fixture.Create<string>();
            var column = fixture.Create<IEnumerable<Aocolumn>>();
            columnInterface.Setup(c => c.GetColumnsByType(name)).ReturnsAsync(column);

            //Act
            var result = columnController.GetColumnsByType(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            columnInterface.Verify(t => t.GetColumnsByType(name), Times.Once());
        }



       

        [Fact]
        public async Task GetColumnsByType_ShouldReturnNotFound_WhenDataNotFound()
        {
            // Arrange
            var name = fixture.Create<string>();
            columnInterface.Setup(c => c.GetColumnsByType(name)).Returns(Task.FromResult<IEnumerable<Aocolumn>>(null));

            // Act
            var result = await columnController.GetColumnsByType(name) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("Columns not found");

            columnInterface.Verify(t => t.GetColumnsByType(name), Times.Once());
        }




       
        [Fact]
        public async Task GetColumnsByType_ShouldReturnBadRequestObjectResult_WhenAnExceptionOccurred()
        {
            // Arrange
            var name = fixture.Create<string>();
            var exceptionMessage = "ExpectedExceptionMessage"; 
            columnInterface.Setup(c => c.GetColumnsByType(name)).Throws(new Exception(exceptionMessage)); 

            // Act
            var result = await columnController.GetColumnsByType(name) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var actualExceptionMessage = result.Value as string;
            actualExceptionMessage.Should().NotBeNull();
            actualExceptionMessage.Should().Be(exceptionMessage); 

            columnInterface.Verify(t => t.GetColumnsByType(name), Times.Once());
        }


    }
}
