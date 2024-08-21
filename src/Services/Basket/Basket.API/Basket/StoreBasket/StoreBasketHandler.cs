using Discount.Grpc.Protos;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
    }
}

public class StoreBasketCommandHandler 
    (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // Todo: Communicate with Discount.Grpc and calculate latest prices of product 
        await DeductDiscount(command.Cart, cancellationToken);

        // TODO: store basket in database (use Marten upsert - if exists update, else insert) update cache
        await repository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }

    public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var items in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = items.ProductName }, cancellationToken: cancellationToken);
            items.Price -= coupon.Amount;
        }
    }
}