# API Usage Checklist

Base URL example: `http://localhost:5000`

## 1. Register accounts (self-service)
Customer:
`POST /api/auth/register/customer`

```json
{
  "fullName": "Alice Martin",
  "email": "alice@example.com",
  "phoneNumber": "+33630000000",
  "password": "Customer123!",
  "street": "5 Rue Oberkampf",
  "city": "Paris",
  "zipCode": "75011"
}
```

Courier:
`POST /api/auth/register/courier`

```json
{
  "fullName": "Yanis Rider",
  "email": "yanis@example.com",
  "phoneNumber": "+33640000000",
  "vehicle": 1,
  "password": "Courier123!"
}
```

## 2. Admin creates store address
`POST /api/addresses`

```json
{
  "street": "10 Avenue des Champs-Elysees",
  "city": "Paris",
  "zipCode": "75008",
  "customerId": null
}
```

## 3. Admin creates store and products
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

## 4. Admin updates/deletes stores
- `PUT /api/stores/{storeId}`
- `DELETE /api/stores/{storeId}`

## 5. Customer creates order
`POST /api/orders`

```json
{
  "storeId": 1,
  "deliveryAddressId": 1,
  "items": [
    { "productId": 1, "quantity": 2 }
  ]
}
```

## 6. Courier handles delivery
- `PATCH /api/orders/{orderId}/assign-courier`
- `PUT /api/orders/{orderId}`

```json
{
  "status": 6,
  "courierId": 1
}
```

## 7. Customer pays and adds review
Payment:
`POST /api/orders/{orderId}/payments`

```json
{
  "method": 0
}
```

Review:
`POST /api/orders/{orderId}/reviews`

```json
{
  "rating": 5,
  "comment": "Tres bonne livraison."
}
```

## 8. Admin moderates accounts
- `GET /api/auth/admin/accounts`
- `PATCH /api/auth/admin/accounts/{accountId}/status`

```json
{
  "isActive": false
}
```
