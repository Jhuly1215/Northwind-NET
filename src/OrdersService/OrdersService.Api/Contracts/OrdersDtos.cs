namespace OrdersService.Api.Contracts;

public record OrderListItemDto(
    int id,
    int? customer_id,
    DateTime? order_date,
    int? status_id
);

public record OrderItemDto(
    int id,
    int? product_id,
    string? product_name,
    decimal? quantity,
    decimal? unit_price,
    int? status_id
);

public record OrderDetailsDto(
    int id,
    int? customer_id,
    int? employee_id,
    DateTime? order_date,
    DateTime? shipped_date,
    int? shipper_id,
    string? ship_name,
    string? ship_city,
    decimal? shipping_fee,
    decimal? taxes,
    int? status_id,
    IReadOnlyList<OrderItemDto> items
);
