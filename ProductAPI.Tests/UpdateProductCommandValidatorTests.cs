using FluentValidation.TestHelper;
using ProductAPI.Commands;
using ProductAPI.Validators;

namespace ProductAPI.Tests
{
    [TestClass]
    public class UpdateProductCommandValidatorTests
    {
        private UpdateProductCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpdateProductCommandValidator();
        }

        [TestMethod]
        public void ShouldNotHaveError()
        {
            var model = new UpdateProductCommand
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

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void ShouldHaveErrorWhenProductIdIsNotPositive(int productId)
        {
            var model = new UpdateProductCommand { ProductId = productId };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
            result.ShouldNotHaveValidationErrorFor(x => x.Stock);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [DataTestMethod]
        [DataRow(2)]
        [DataRow(-1)]
        public void ShouldHaveErrorWhenStatusIsNotZeroOrOne(int status)
        {
            var model = new UpdateProductCommand { Status = status };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.Stock);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-2.5)]
        public void ShouldHaveErrorWhenPriceIsNotPositive(double price)
        {
            var model = new UpdateProductCommand { Price = price };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
            result.ShouldNotHaveValidationErrorFor(x => x.Stock);
        }

        public void ShouldHaveErrorWhenStockIsNegative()
        {
            var model = new UpdateProductCommand { Stock = -10 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Stock);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }
    }
}
