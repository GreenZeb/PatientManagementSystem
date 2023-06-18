using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PatientManagementSystem.Controllers;
using PatientManagementSystem.Core.Models;
using PatientManagementSystem.Data;

namespace PatientManagementSystem.Tests
{
    public class ApiControllerTests
    {
        private ApplicationDbContext _context;
        private ApiController _controller;

        [SetUp]
        public void Setup()
        {
            SetupDatabase();
            _controller = new ApiController(_context);
        }

        [Test]
        public async Task GetAllPatients_ReturnsAllPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, Name = "John" },
                new Patient { Id = 2, Name = "Jane" }
            };
            _context.Patients.AddRange(patients);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            var okResult = result as OkObjectResult;
            var resultPatients = okResult.Value as List<Patient>;
            resultPatients.Should().HaveCount(2);
            resultPatients.Select(p => p.Name).Should().ContainInOrder("John", "Jane");
        }

        [Test]
        public async Task GetDoctorPatients_ValidDoctorId_ReturnsDoctorPatients()
        {
            // Arrange
            var doctorId = 1;
            var patients = new List<Patient>
            {
                new Patient { Id = 1, Name = "John", DoctorId = doctorId },
                new Patient { Id = 2, Name = "Jane", DoctorId = doctorId },
                new Patient { Id = 3, Name = "Alice", DoctorId = 2 }
            };
            _context.Patients.AddRange(patients);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetDoctorPatients(doctorId);

            // Assert
            var okResult = result as OkObjectResult;
            var resultPatients = okResult.Value as List<Patient>;
            resultPatients.Should().HaveCount(2);
            resultPatients.Select(p => p.Name).Should().ContainInOrder("John", "Jane");
            resultPatients.Should().OnlyContain(p => p.DoctorId == doctorId);
        }

        [Test]
        public async Task AddPatient_ValidPatient_ReturnsCreatedResponse()
        {
            // Arrange
            var patient = new Patient { Name = "John" };

            // Act
            var result = await _controller.AddPatient(patient);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.ActionName.Should().Be(nameof(_controller.GetPatient));
            createdResult.RouteValues["id"].Should().Be(patient.Id); // Updated assertion
            createdResult.Value.Should().BeEquivalentTo(patient);
        }

        [Test]
        public async Task GetPatient_ExistingPatientId_ReturnsPatient()
        {
            // Arrange
            var patient = new Patient { Id = 1, Name = "John" };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetPatient(patient.Id);

            // Assert
            var okResult = result as OkObjectResult;
            var resultPatient = okResult.Value as Patient;
            resultPatient.Should().NotBeNull();
            resultPatient.Id.Should().Be(patient.Id);
            resultPatient.Name.Should().Be(patient.Name);
        }

        [Test]
        public async Task GetPatient_NonExistingPatientId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingPatientId = 100;

            // Act
            var result = await _controller.GetPatient(nonExistingPatientId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        private void SetupDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;
            _context = new ApplicationDbContext(options);
        }
    }
}