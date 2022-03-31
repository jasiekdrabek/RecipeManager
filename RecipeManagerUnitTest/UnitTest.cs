using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeManager.DataAccess;

namespace RecipeManagerUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ValidatePassword_CorrectData()
        {
            string password = "Aq1dq431DWEEF";
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void ValidatePassword_WithoutNumbers()
        {
            string password = "AqdqDWEEF";
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidatePassword_WithoutSmallLetters()
        {
            string password = "A1431DWEEF";
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidatePassword_WithoutBigLetters()
        {
            string password = "q1dq431";
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidatePassword_Null()
        {
            string password = null;
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidatePassword_EmptyString()
        {
            string password = string.Empty;
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidatePassword_TooLong()
        {
            string password = "Aq1aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var validation = new UserValidation();
            bool result = validation.ValidatePassword(password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateLogin_Null()
        {
            string login = null;
            var validation = new UserValidation();
            bool result = validation.ValidateLogin(login);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateLogin_EmptyString()
        {
            string login = string.Empty;
            var validation = new UserValidation();
            bool result = validation.ValidateLogin(login);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateLogin_TooLong()
        {
            string login = "jan123456789012345678";
            var validation = new UserValidation();
            bool result = validation.ValidateLogin(login);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateLogin_CorrectData()
        {
            string login = "jan";
            var validation = new UserValidation();
            bool result = validation.ValidateLogin(login);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void NameValidation_CorrectData()
        {
            string recipe = "jan";
            var validation = new RecipeValidation();
            bool result = validation.NameValidation(recipe);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void NameValidation_Null()
        {
            string recipe = null;
            var validation = new RecipeValidation();
            bool result = validation.NameValidation(recipe);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void NameValidation_EmptyString()
        {
            string recipe = string.Empty;
            var validation = new RecipeValidation();
            bool result = validation.NameValidation(recipe);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateQuantity_EmptyString()
        {
            string quanity = string.Empty;
            var validation = new ProductValidation();
            bool result = validation.ValidateQuantity(quanity);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateQuantity_Null()
        {
            string quanity = null;
            var validation = new ProductValidation();
            bool result = validation.ValidateQuantity(quanity);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateQuantity_NotNumber()
        {
            string quanity = "somestring";
            var validation = new ProductValidation();
            bool result = validation.ValidateQuantity(quanity);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateQuantity_NotIntNumber()
        {
            string quanity = "11.5";
            var validation = new ProductValidation();
            bool result = validation.ValidateQuantity(quanity);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateQuantity_NegativeNumber()
        {
            string quanity = "-4";
            var validation = new ProductValidation();
            bool result = validation.ValidateQuantity(quanity);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ValidateQuantity_CorrectData()
        {
            string quanity = "5";
            var validation = new ProductValidation();
            bool result = validation.ValidateQuantity(quanity);
            Assert.AreEqual(true, result);
        }
    }
}
