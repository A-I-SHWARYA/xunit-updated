using AutoFixture;
using Moq;
using FluentAssertions;
using XunitAssessment.Services.Interfaces;
using XunitAssessment.Controllers;
using Microsoft.AspNetCore.Mvc;
using XunitAssessment.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tests.Controllers
{
    public class AoTableControllerTest
    {



        private readonly IFixture fixture;
        private readonly Mock<AotableInterface> tableInterface;
        private readonly AoTableController tableController;


        

        public AoTableControllerTest()
        {
            fixture = new Fixture();
            fixture.Customize<Aotable>(composer => composer.Without(t => t.Aocolumns));
            tableInterface = fixture.Freeze<Mock<AotableInterface>>();
            tableController = new AoTableController(tableInterface.Object);
        }


        //Add record
       

        [Fact]
        public async Task AddTable_ShouldReturnOk_WhenSuccess()
        {
            // Arrange
            var table = fixture.Create<Aotable>();
            var returnData = fixture.Create<Aotable>();
            tableInterface.Setup(t => t.AddTable(table)).ReturnsAsync(returnData);

            // Act
            var result = await tableController.AddTable(table) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            var addedData = result.Value as Aotable;
            addedData.Should().NotBeNull();

            addedData.Should().BeEquivalentTo(returnData); // This checks that the added data is equivalent to the return data

            tableInterface.Verify(t => t.AddTable(table), Times.Once());
        }


        [Fact]
        public void AddTable_ShouldReturnBadRequest_WhenTableObjectIsNull()
        {
            //Arrange
            Aotable table = null;
            tableInterface.Setup(t => t.AddTable(table)).ReturnsAsync((Aotable)null);

            //Act
            var result = tableController.AddTable(table);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            tableInterface.Verify(t => t.AddTable(table), Times.Never());
        }


       
        [Fact]
        public async Task AddTable_ShouldReturnBadRequestObjectResult_WhenAddFailed()
        {
            // Arrange
            var table = fixture.Create<Aotable>();
            tableInterface.Setup(t => t.AddTable(table)).Returns(Task.FromResult<Aotable>(null));

            // Act
            var result = await tableController.AddTable(table) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("Table Cannot be null");

            tableInterface.Verify(t => t.AddTable(table), Times.Once());
        }


        

        [Fact]
        public async Task AddTable_ShouldReturnBadRequestObjectResult_WhenAnExceptionOccurred()
        {
            // Arrange
            var table = fixture.Create<Aotable>();
            var exceptionMessage = "ExpectedExceptionMessage"; 
            tableInterface.Setup(t => t.AddTable(table)).Throws(new Exception(exceptionMessage)); 

            // Act
            var result = await tableController.AddTable(table) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var actualExceptionMessage = result.Value as string;
            actualExceptionMessage.Should().NotBeNull();
            actualExceptionMessage.Should().Be(exceptionMessage); 

            tableInterface.Verify(t => t.AddTable(table), Times.Once());
        }




        //Edit a record
        

        [Fact]
        public async Task EditTable_ShouldReturnOk_WhenEditSuccess()
        {
            // Arrange
            Guid id = fixture.Create<Guid>();
            var table = fixture.Create<Aotable>();
            var returnData = fixture.Create<Aotable>();
            tableInterface.Setup(t => t.EditTable(id, table)).ReturnsAsync(returnData);

            // Act
            var result = await tableController.EditTable(id, table) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            var editedData = result.Value as Aotable;
            editedData.Should().NotBeNull();

            editedData.Should().BeEquivalentTo(returnData); // This checks that the edited data is equivalent to the return data

            tableInterface.Verify(t => t.EditTable(id, table), Times.Once());
        }



        [Fact]
        public void EditTable_ShouldReturnBadRequestResult_WhenEditFailed()
        {
            //Arrange
            Guid id = fixture.Create<Guid>();
            Aotable table = null;
            tableInterface.Setup(t => t.EditTable(id, table)).ReturnsAsync((Aotable)null);

            //Act
            var result = tableController.EditTable(id, table);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            tableInterface.Verify(t => t.EditTable(id, table), Times.Never());
        }

        

        [Fact]
        public async Task EditTable_ShouldReturnNotFoundObjectResult_WhenDataNotFound()
        {
            // Arrange
            Guid id = fixture.Create<Guid>();
            Aotable table = fixture.Create<Aotable>();
            tableInterface.Setup(t => t.EditTable(id, table)).Returns(Task.FromResult<Aotable>(null));

            // Act
            var result = await tableController.EditTable(id, table) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("Record cannot be editted!");

            tableInterface.Verify(t => t.EditTable(id, table), Times.Once());
        }


        
        [Fact]
        public async Task EditTable_ShouldReturnBadRequestObjectResult_WhenAnExceptionOccurred()
        {
            // Arrange
            Guid id = fixture.Create<Guid>();
            Aotable table = fixture.Create<Aotable>();
            var exceptionMessage = "ExpectedExceptionMessage"; 
            tableInterface.Setup(t => t.EditTable(id, table)).Throws(new Exception(exceptionMessage)); 
            // Act
            var result = await tableController.EditTable(id, table) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var actualExceptionMessage = result.Value as string;
            actualExceptionMessage.Should().NotBeNull();
            actualExceptionMessage.Should().Be(exceptionMessage); 
            tableInterface.Verify(t => t.EditTable(id, table), Times.Once());
        }










        //Get all records which has a particular word in the Name. The word needs to be a parameter.
        [Fact]
        public void CheckName_ShouldReturnOk_WhenSuccess()
        {
            //Arrange
            var name = fixture.Create<string>();
            var table = fixture.Create<IEnumerable<Aotable>>();
            tableInterface.Setup(c => c.CheckName(name)).ReturnsAsync(table);

            //Act
            var result = tableController.CheckName(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            tableInterface.Verify(t => t.CheckName(name), Times.Once());
        }

        [Fact]
        public async Task CheckName_ShouldRetrnBadRequest_WhenNameIsNull()
        {
            // Arrange
            string name = null;

            // Act
            var result = await tableController.CheckName(name) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("The table name cannot be empty.");

            tableInterface.Verify(t => t.CheckName(name), Times.Never());
        }








        

        [Fact]
        public void CheckName_ShouldRetrnBadRequest_WhenNameIsEmpty()
        {
            //Arrange
            string name = "";
            tableInterface.Setup(c => c.CheckName(name));

            //Act
            var result = tableController.CheckName(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            tableInterface.Verify(t => t.CheckName(name), Times.Never());
        }
        


        


        [Fact]
        public async Task CheckName_ShouldReturnNotFound_WhenDataNotFound()
        {
            // Arrange
            var name = fixture.Create<string>();
            tableInterface.Setup(c => c.CheckName(name)).Returns(Task.FromResult<IEnumerable<Aotable>>(null));

            // Act
            var result = await tableController.CheckName(name) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("The record with the particular name not found");

            tableInterface.Verify(t => t.CheckName(name), Times.Once());
        }

        
        [Fact]
        public async Task CheckName_ShouldReturnBadRequestObjectResult_WhenAnExceptionOccurred()
        {
            // Arrange
            var name = fixture.Create<string>();
            var exceptionMessage = "ExpectedExceptionMessage"; 
            tableInterface.Setup(c => c.CheckName(name)).Throws(new Exception(exceptionMessage)); 
            // Act
            var result = await tableController.CheckName(name) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var actualExceptionMessage = result.Value as string;
            actualExceptionMessage.Should().NotBeNull();
            actualExceptionMessage.Should().Be(exceptionMessage); 

            tableInterface.Verify(t => t.CheckName(name), Times.Once());
        }












        //Get all records of Type "coverage" and "form". This Type needs to be a parameter.
        [Fact]
        public void GetByType_ShouldReturnOk_WhenSuccess()
        {
            //Arrange
            var type = fixture.Create<string>();
            var table = fixture.Create<IEnumerable<Aotable>>();
            tableInterface.Setup(c => c.GetByType(type)).ReturnsAsync(table);

            //Act
            var result = tableController.GetByType(type);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Task<IActionResult>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            tableInterface.Verify(t => t.GetByType(type), Times.Once());
        }


       

        [Fact]
        public async Task GetByType_ShouldReturnNotFound_WhenDataNotFound()
        {
            // Arrange
            var type = fixture.Create<string>();
            tableInterface.Setup(c => c.GetByType(type)).Returns(Task.FromResult<IEnumerable<Aotable>>(null));

            // Act
            var result = await tableController.GetByType(type) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();

            var errorMessage = result.Value as string;
            errorMessage.Should().NotBeNull();
            errorMessage.Should().Be("Tables not found");

            tableInterface.Verify(t => t.GetByType(type), Times.Once());
        }


       
        [Fact]
        public async Task GetByType_ShouldReturnBadRequestObjectResult_WhenAnExceptionOccurred()
        {
            // Arrange
            var type = fixture.Create<string>();
            var exceptionMessage = "ExpectedExceptionMessage"; 
            tableInterface.Setup(c => c.GetByType(type)).Throws(new Exception(exceptionMessage)); 
            // Act
            var result = await tableController.GetByType(type) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();

            var actualExceptionMessage = result.Value as string;
            actualExceptionMessage.Should().NotBeNull();
            actualExceptionMessage.Should().Be(exceptionMessage); 

            tableInterface.Verify(t => t.GetByType(type), Times.Once());
        }



    }
}


    
