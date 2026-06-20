using BlazorBootstrap;
using Database.Entities;
using DTO.DTOEntities;
using System.Text.Json;

namespace UserInterface.Services
{
    public class ProductInfoService
    {
        private readonly SendRequestService _sendRequestService;

        private readonly RoleService _roleService;

        private readonly ForceExitService _forceExitService;

        public ProductInfoService(SendRequestService sendRequestService, RoleService roleService, ForceExitService forceExitService)
        {
            _sendRequestService = sendRequestService;
            _roleService = roleService;
            _forceExitService = forceExitService;
        }

        public event Action? OnChange;

        public List<ProductDTO>? productsDTO { get; private set; }

        public List<CartItemDTO> allCartItemsInCartDTO { get; private set; } = new();

        public bool isAdmin { get; private set; }

        public async Task LoadDataAsync(string? searchtext = null)
        {
            try
            {
                string requestUri = "api/product/getall";
                if (!String.IsNullOrWhiteSpace(searchtext))
                {
                    requestUri = $"api/product/getbytext?searchtext={searchtext}";
                }

                using var response = await _sendRequestService.SendAsync(HttpMethod.Get, requestUri);

                if (response!.IsSuccessStatusCode)
                {
                    try
                    {
                        using var responseStream = await response.Content.ReadAsStreamAsync();
                        productsDTO = (await JsonSerializer.DeserializeAsync<List<ProductDTO>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))!;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR {ex.Message}");
                        await _forceExitService.Exit();
                    }
                }

                using var response2 = await _sendRequestService.SendAsync(HttpMethod.Get, "api/cartitem/getall");

                if (response2!.IsSuccessStatusCode)
                {
                    try
                    {
                        using var responseStream = await response2.Content.ReadAsStreamAsync();
                        allCartItemsInCartDTO = (await JsonSerializer.DeserializeAsync<List<CartItemDTO>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))!;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR {ex.Message}");
                        await _forceExitService.Exit();
                    }
                }

                isAdmin = await _roleService.IsAdmin();

                OnChange?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR {ex.Message}");
                await _forceExitService.Exit();
            }
        }

        public CartItemDTO? GetCartItemForProduct(int prodId)
        {
            return allCartItemsInCartDTO!.FirstOrDefault(x => x.ProductId == prodId);
        }

        public decimal GetCartTotalPrice()
        {
            decimal TotalPrice = 0;
            foreach(var prod in productsDTO!)
            {
                var cartItem = GetCartItemForProduct(prod.Id);
                if(cartItem is not null)
                {
                    TotalPrice += prod.Price * cartItem.Quantity;
                }
            }

            return TotalPrice;
        }

        public async Task UpdateCartItem(CartItemDTO cartItem, bool IsPlus)
        {
            cartItem.Quantity = IsPlus ? cartItem.Quantity + 1 : cartItem.Quantity - 1;

            if (cartItem.Quantity == 0)
            {
                await _sendRequestService.SendAsync(HttpMethod.Delete, $"api/CartItem/delete/{cartItem.Id}");
            }
            else
            {
                var json = JsonSerializer.Serialize(cartItem);

                using var response = await _sendRequestService.SendAsync(HttpMethod.Put, "api/CartItem/update", json);
            }

            await LoadDataAsync();
        }

        public async Task ClickAddToCart(int productId)
        {
            CartItemDTO cartItem = new CartItemDTO { ProductId = productId, Quantity = 1 };

            var json = JsonSerializer.Serialize(cartItem);

            using var response = await _sendRequestService.SendAsync(HttpMethod.Post, "api/CartItem/addnew", json);

            if (response!.IsSuccessStatusCode)
            {
                await LoadDataAsync();
            }
        }

        public async Task ClickDelete(int id)
        {
            using var response = await _sendRequestService.SendAsync(HttpMethod.Delete, $"api/product/delete/{id}");

            if (response!.IsSuccessStatusCode)
            {
                await LoadDataAsync();
            }
        }

        public async Task<ProductDTO?> ClickEdit(int id)
        {
            using var response = await _sendRequestService.SendAsync(HttpMethod.Get, $"api/product/getbyid/{id}");

            if (response!.IsSuccessStatusCode)
            {
                try
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    ProductDTO product = (await JsonSerializer.DeserializeAsync<ProductDTO>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))!;
                    return product;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR {ex.Message}");
                    await _forceExitService.Exit();
                }
            }
            return null;
        }

        public async Task SaveModalClickAsync(ProductDTO product)
        {
            var json = JsonSerializer.Serialize(product);

            using var response = await _sendRequestService.SendAsync(HttpMethod.Put, "api/product/update", json);

            await LoadDataAsync();
        }

        public async Task ClearCart()
        {
            foreach(var cartItem in allCartItemsInCartDTO)
            {
                await _sendRequestService.SendAsync(HttpMethod.Delete, $"api/cartitem/delete/{cartItem.Id}");
            }

            allCartItemsInCartDTO.Clear();
        }
    }
}
