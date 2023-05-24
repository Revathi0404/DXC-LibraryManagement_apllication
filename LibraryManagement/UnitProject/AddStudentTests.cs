using System.Data.SqlClient;
using LibraryManagement;
using Moq;
using NUnit.Framework;
using Unity;

namespace UnitProject
{
    public class AddStudentTests
    {


        [Test]
        public void AddStudents_WhenCalled_ReturnsValues()
        {
           // Arrange
           var add = new Mock<IStudent>();
           add.Setup(d => d.AddStudent()).Returns(1);

           // Act
           int result = add.Object.AddStudent();

           // Assert
           result
        }


    }
}
       