namespace OrdersService.Api.Contracts;

public record CreateOrderItemRequest(
    int? product_id,
    decimal? quantity,
    decimal? unit_price,
    int? status_id
);

public record CreateOrderRequest(
    int? customer_id,
    int? employee_id,
    int? shipper_id,
    string? ship_name,
    string? ship_city,
    int? status_id,
    List<CreateOrderItemRequest> items
);

public record UpdateOrderRequest(
    int? customer_id,
    int? employee_id,
    int? shipper_id,
    string? ship_name,
    string? ship_city,
    int? status_id,
    List<CreateOrderItemRequest>? items
);

public record PatchOrderStatusRequest(int status_id);
