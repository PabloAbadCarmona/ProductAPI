using FluentValidation.TestHelper;
using ProductAPI.Commands;
using ProductAPI.Validators;

namespace ProductAPI.Tests
{
    [TestClass]
    public class CreateProductCommandValidatorTests
    {
        private CreateProductCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new CreateProductCommandValidator();
        }

        [TestMethod]
        public void ShouldNotHaveError()
        {
            var model = new CreateProductCommand
            {
                ProductId = 1,
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void ShouldHaveErrorWhenNameAndDescriptionAreEmpty()
        {
            var model = new CreateProductCommand { Name = "", Description = ""};
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void ShouldHaveErrorWhenProductIdIsNotPositive(int productId)
        {
            var model = new CreateProductCommand { ProductId = productId, Name = "", Description = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }

        [DataTestMethod]
        [DataRow(2)]
        [DataRow(-1)]
        public void ShouldHaveErrorWhenStatusIsNotZeroOrOne(int status)
        {
            var model = new CreateProductCommand { Status = status, Name = "", Description = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-2.5)]
        public void ShouldHaveErrorWhenPriceIsNotPositive(double price)
        {
            var model = new CreateProductCommand { Price = price, Name = "", Description = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }

        public void ShouldHaveErrorWhenStockIsNegative()
        {
            var model = new CreateProductCommand { Stock = -10, Name = "", Description = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Stock);
        }
    }
}
