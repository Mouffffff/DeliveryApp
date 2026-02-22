# API Simulation Checklist

Base URL example: `https://localhost:5001`

## 1. Create customer
`POST /api/customers`

```json
{
  "fullName": "Alice Martin",
  "email": "alice@example.com",
  "phoneNumber": "+33630000000"
}
```

## 2. Create customer address
`POST /api/addresses`

```json
{
  "street": "5 Rue Oberkampf",
  "city": "Paris",
  "zipCode": "75011",
  "customerId": 1
}
```

## 3. Create store and products
`POST /api/stores`

```json
{
  "name": "Pizza House",
  "category": "Italian",
  "addressId": 2
}
```

`POST /api/products`

```json
{
  "name": "Pizza Regina",
  "price": 14.9,
  "storeId": 1
}
```

## 4. Create courier
`POST /api/couriers`

```json
{
  "fullName": "Yanis Rider",
  "phoneNumber": "+33640000000",
  "vehicle": 1
}
```

## 5. Create order
`POST /api/orders`

```json
{
  "customerId": 1,
  "storeId": 1,
  "deliveryAddressId": 1,
  "items": [
    { "productId": 1, "quantity": 2 }
  ]
}
```

## 6. Assign courier and progress order
- `PATCH /api/orders/{orderId}/assign-courier`
- `PUT /api/orders/{orderId}`

```json
{
  "status": 6,
  "courierId": 1
}
```

## 7. Pay order
`POST /api/orders/{orderId}/payments`

```json
{
  "amount": 29.8,
  "method": 0
}
```

## 8. Add review
`POST /api/orders/{orderId}/reviews`

```json
{
  "rating": 5,
  "comment": "Tres bonne livraison."
}
```
