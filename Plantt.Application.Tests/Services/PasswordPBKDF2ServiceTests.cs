using Microsoft.Extensions.Options;
using Moq;
using Plantt.Applcation.Services;
using Plantt.Domain.Config;
using Plantt.Domain.Models;

namespace Plantt.Application.Tests.Services
{
    public class PasswordPBKDF2ServiceTests
    {
        private readonly Mock<IOptions<PasswordSettings>> _passwordSettingsMock;

        private readonly PasswordPBKDF2Service subject;

        public PasswordPBKDF2ServiceTests()
        {
            _passwordSettingsMock = new Mock<IOptions<PasswordSettings>>();

            _passwordSettingsMock.Setup(opotion => opotion.Value)
                .Returns(new PasswordSettings
                {
                    SaltSize = 32,
                    PasswordHashSize = 64,
                    MinIterations = 10000,
                    MaxIterations = 50000
                });

            subject = new PasswordPBKDF2Service(_passwordSettingsMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreatePassword_Throws_ArgumentNullException_If_Password_Is_Null_Or_Empty(string password)
        {
            // Arrange

            // Act
            Action act = () => subject.CreatePassword(password);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void CreatePassword_Returns_Password_With_Correct_Lengths()
        {
            // Arrange
            var password = "password";
            PasswordSettings settings = _passwordSettingsMock.Object.Value;

            // Act
            var result = subject.CreatePassword(password);

            // Assert
            Assert.Equal(settings.PasswordHashSize, result.HashedPassword.Length);
            Assert.Equal(settings.SaltSize, result.Salt.Length);
        }


        [Fact]
        public void VerifyPassword_Returns_True_If_Password_Is_Valid()
        {
            // Arrange
            var password = "password";
            var storedPassword = subject.CreatePassword(password);

            // Act
            var result = subject.VerifyPassword(password, storedPassword);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void VerifyPassword_Returns_False_If_Password_Is_Invalid()
        {
            // Arrange
            var password = "password";
            var storedPassword = subject.CreatePassword(password);

            // Act
            var result = subject.VerifyPassword("wrong_password", storedPassword);

            // Assert
            Assert.False(result);
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("password", null)]
        [InlineData(null, new byte[] { 1, 2, 3 })]
        [InlineData("", new byte[] { 1, 2, 3 })]
        public void VerifyPassword_Throws_ArgumentNullException_If_Password_Or_HashedPassword_Is_Null_Or_Empty(string password, byte[] hashedPassword)
        {
            // Arrange
            var passwordObject = new Password()
            {
                HashedPassword = hashedPassword,
                Salt = new byte[] { 4, 5, 6 },
                Iterations = 10000
            };

            // Act
            Action act = () => subject.VerifyPassword(password, passwordObject);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
