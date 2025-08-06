using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator: AbstractValidator<VM_Create_Product>
    {

        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Ürün adı boş geçilemez.")
                .MinimumLength(2)
                .MaximumLength(150)
                .WithMessage("Ürün adı 2 ile 150 karakter arasında olmalıdır.");

            RuleFor(p => p.Stock)
                .NotEmpty().WithMessage("Stok bilgisi boş geçilemez.")
                .GreaterThanOrEqualTo(0).WithMessage("Stok bilgisi 0 veya daha büyük olmalıdır.");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Fiyat bilgisi boş geçilemez.")
                .GreaterThan(0).WithMessage("Fiyat bilgisi 0'dan büyük olmalıdır.");

        }
    }
}
