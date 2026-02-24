using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DeliveryApp.Frontend.Models;

namespace DeliveryApp.Frontend.Services;

public sealed class DeliveryApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly AuthSessionState _session;

    public DeliveryApiClient(HttpClient httpClient, AuthSessionState session)
    {
        _httpClient = httpClient;
        _session = session;
    }

    public async Task<ApiResult<HealthResponse>> GetHealthAsync() =>
        await GetAsync<HealthResponse>("/health", withAuth: false);

    public async Task<ApiResult<AuthTokenResponse>> LoginAsync(LoginRequest request) =>
        await PostAsync<LoginRequest, AuthTokenResponse>("/api/auth/login", request, withAuth: false);

    public async Task<ApiResult<AuthTokenResponse>> RegisterCustomerAccountAsync(RegisterCustomerAccountRequest request) =>
        await PostAsync<RegisterCustomerAccountRequest, AuthTokenResponse>("/api/auth/register/customer", request, withAuth: false);

    public async Task<ApiResult<AuthTokenResponse>> RegisterCourierAccountAsync(RegisterCourierAccountRequest request) =>
        await PostAsync<RegisterCourierAccountRequest, AuthTokenResponse>("/api/auth/register/courier", request, withAuth: false);

    public async Task<ApiResult<AuthTokenResponse>> RegisterAdminAccountAsync(RegisterAdminAccountRequest request) =>
        await PostAsync<RegisterAdminAccountRequest, AuthTokenResponse>("/api/auth/register/admin", request, withAuth: false);

    public async Task<ApiResult<AccountContextResponse>> GetAccountContextAsync() =>
        await GetAsync<AccountContextResponse>("/api/account/context");

    public async Task<ApiResult<IReadOnlyList<AddressDto>>> GetMyAddressesAsync() =>
        await GetAsync<IReadOnlyList<AddressDto>>("/api/account/addresses");

    public async Task<ApiResult<AddressDto>> CreateMyAddressAsync(CreateAddressRequest request) =>
        await PostAsync<CreateAddressRequest, AddressDto>("/api/account/addresses", request);

    public async Task<ApiResult<IReadOnlyList<StoreDto>>> GetStoresAsync() =>
        await GetAsync<IReadOnlyList<StoreDto>>("/api/stores", withAuth: false);

    public async Task<ApiResult<StoreDto>> CreateStoreAsync(CreateStoreRequest request) =>
        await PostAsync<CreateStoreRequest, StoreDto>("/api/stores", request);

    public async Task<ApiResult<StoreDto>> UpdateStoreAsync(int storeId, UpdateStoreRequest request) =>
        await PutAsync<UpdateStoreRequest, StoreDto>($"/api/stores/{storeId}", request);

    public async Task<ApiResult<bool>> DeleteStoreAsync(int storeId) =>
        await DeleteAsync($"/api/stores/{storeId}");

    public async Task<ApiResult<IReadOnlyList<ProductDto>>> GetStoreProductsAsync(int storeId) =>
        await GetAsync<IReadOnlyList<ProductDto>>($"/api/stores/{storeId}/products", withAuth: false);

    public async Task<ApiResult<IReadOnlyList<CustomerDto>>> GetCustomersAsync() =>
        await GetAsync<IReadOnlyList<CustomerDto>>("/api/customers");

    public async Task<ApiResult<IReadOnlyList<AddressDto>>> GetAddressesAsync() =>
        await GetAsync<IReadOnlyList<AddressDto>>("/api/addresses");

    public async Task<ApiResult<AddressDto>> CreateAddressAsync(CreateAddressRequest request) =>
        await PostAsync<CreateAddressRequest, AddressDto>("/api/addresses", request);

    public async Task<ApiResult<IReadOnlyList<AdminUserAccountDto>>> GetAdminAccountsAsync() =>
        await GetAsync<IReadOnlyList<AdminUserAccountDto>>("/api/auth/admin/accounts");

    public async Task<ApiResult<AdminUserAccountDto>> SetAccountStatusAsync(int accountId, bool isActive) =>
        await PatchAsync<SetAccountStatusRequest, AdminUserAccountDto>(
            $"/api/auth/admin/accounts/{accountId}/status",
            new SetAccountStatusRequest(isActive));

    public async Task<ApiResult<IReadOnlyList<CourierDto>>> GetCouriersAsync() =>
        await GetAsync<IReadOnlyList<CourierDto>>("/api/couriers");

    public async Task<ApiResult<IReadOnlyList<CourierDto>>> GetAvailableCouriersAsync() =>
        await GetAsync<IReadOnlyList<CourierDto>>("/api/couriers/available");

    public async Task<ApiResult<DeliveryDto>> CreateOrderAsync(CreateOrderRequest request) =>
        await PostAsync<CreateOrderRequest, DeliveryDto>("/api/orders", request);

    public async Task<ApiResult<IReadOnlyList<DeliveryDto>>> GetOrdersAsync() =>
        await GetAsync<IReadOnlyList<DeliveryDto>>("/api/orders");

    public async Task<ApiResult<IReadOnlyList<DeliveryDto>>> GetMyOrdersAsync() =>
        await GetAsync<IReadOnlyList<DeliveryDto>>("/api/orders/mine");

    public async Task<ApiResult<IReadOnlyList<DeliveryDto>>> GetOrdersByStatusAsync(int status) =>
        await GetAsync<IReadOnlyList<DeliveryDto>>($"/api/orders/status/{status}");

    public async Task<ApiResult<DeliveryDto>> GetOrderAsync(int orderId) =>
        await GetAsync<DeliveryDto>($"/api/orders/{orderId}");

    public async Task<ApiResult<DeliveryDto>> AssignCourierAsync(int orderId, int courierId) =>
        await PatchAsync<AssignCourierRequest, DeliveryDto>($"/api/orders/{orderId}/assign-courier", new AssignCourierRequest(courierId));

    public async Task<ApiResult<DeliveryDto>> UpdateOrderAsync(int orderId, int status, int? courierId) =>
        await PutAsync<UpdateOrderRequest, DeliveryDto>($"/api/orders/{orderId}", new UpdateOrderRequest(status, courierId));

    public async Task<ApiResult<PaymentDto>> CreatePaymentAsync(int orderId, int method) =>
        await PostAsync<CreatePaymentRequest, PaymentDto>($"/api/orders/{orderId}/payments", new CreatePaymentRequest(method));

    public async Task<ApiResult<IReadOnlyList<PaymentDto>>> GetPaymentsAsync(int orderId) =>
        await GetAsync<IReadOnlyList<PaymentDto>>($"/api/orders/{orderId}/payments");

    public async Task<ApiResult<ReviewDto>> CreateReviewAsync(int orderId, int rating, string comment) =>
        await PostAsync<CreateReviewRequest, ReviewDto>($"/api/orders/{orderId}/reviews", new CreateReviewRequest(rating, comment));

    public async Task<ApiResult<IReadOnlyList<ReviewDto>>> GetReviewsAsync(int orderId) =>
        await GetAsync<IReadOnlyList<ReviewDto>>($"/api/orders/{orderId}/reviews");

    private async Task<ApiResult<TResponse>> GetAsync<TResponse>(string url, bool withAuth = true)
    {
        try
        {
            using var request = CreateRequest(HttpMethod.Get, url, withAuth);
            using var response = await _httpClient.SendAsync(request);
            return await ReadResponseAsync<TResponse>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<TResponse>.Failure($"Erreur reseau: {ex.Message}");
        }
    }

    private async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest payload, bool withAuth = true)
    {
        try
        {
            using var request = CreateRequest(HttpMethod.Post, url, withAuth);
            request.Content = JsonContent.Create(payload, options: JsonOptions);
            using var response = await _httpClient.SendAsync(request);
            return await ReadResponseAsync<TResponse>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<TResponse>.Failure($"Erreur reseau: {ex.Message}");
        }
    }

    private async Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest payload, bool withAuth = true)
    {
        try
        {
            using var request = CreateRequest(HttpMethod.Put, url, withAuth);
            request.Content = JsonContent.Create(payload, options: JsonOptions);
            using var response = await _httpClient.SendAsync(request);
            return await ReadResponseAsync<TResponse>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<TResponse>.Failure($"Erreur reseau: {ex.Message}");
        }
    }

    private async Task<ApiResult<TResponse>> PatchAsync<TRequest, TResponse>(string url, TRequest payload, bool withAuth = true)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload, JsonOptions);
            using var request = CreateRequest(HttpMethod.Patch, url, withAuth);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request);
            return await ReadResponseAsync<TResponse>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<TResponse>.Failure($"Erreur reseau: {ex.Message}");
        }
    }

    private async Task<ApiResult<bool>> DeleteAsync(string url, bool withAuth = true)
    {
        try
        {
            using var request = CreateRequest(HttpMethod.Delete, url, withAuth);
            using var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return ApiResult<bool>.Success(true);
            }

            var error = await ReadProblemAsync(response);
            return ApiResult<bool>.Failure(error, (int)response.StatusCode);
        }
        catch (Exception ex)
        {
            return ApiResult<bool>.Failure($"Erreur reseau: {ex.Message}");
        }
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string url, bool withAuth)
    {
        var request = new HttpRequestMessage(method, url);
        if (withAuth && _session.IsAuthenticated && !string.IsNullOrWhiteSpace(_session.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _session.AccessToken);
        }

        return request;
    }

    private static async Task<ApiResult<TResponse>> ReadResponseAsync<TResponse>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var value = await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
            if (value is null)
            {
                return ApiResult<TResponse>.Failure("Reponse vide du serveur.");
            }

            return ApiResult<TResponse>.Success(value);
        }

        var error = await ReadProblemAsync(response);
        return ApiResult<TResponse>.Failure(error, (int)response.StatusCode);
    }

    private static async Task<string> ReadProblemAsync(HttpResponseMessage response)
    {
        var raw = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(raw))
        {
            return $"Erreur HTTP {(int)response.StatusCode}.";
        }

        try
        {
            using var document = JsonDocument.Parse(raw);
            var root = document.RootElement;
            if (root.TryGetProperty("detail", out var detail) && detail.ValueKind == JsonValueKind.String)
            {
                return detail.GetString() ?? $"Erreur HTTP {(int)response.StatusCode}.";
            }

            if (root.TryGetProperty("error", out var errorNode) && errorNode.ValueKind == JsonValueKind.String)
            {
                return errorNode.GetString() ?? $"Erreur HTTP {(int)response.StatusCode}.";
            }

            if (root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
            {
                return title.GetString() ?? $"Erreur HTTP {(int)response.StatusCode}.";
            }
        }
        catch
        {
            // Ignore parsing issues and fallback to raw content.
        }

        return raw;
    }
}
