namespace SH.Bookstore.Orders.Host.Mongo.Entities;

public enum OrderState
{
    Created,
    ConfirmTotalPrice,
    WaitingForSupplier,
    Picking,
    OnTheWay,
    Delivered,
    Done
}

