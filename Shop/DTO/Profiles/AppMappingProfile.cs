using Database.Entities;
using DTO.DTOEntities;
using DTO.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Profiles
{
    public class AppMappingProfile : IAppMappingProfile
    {

        public User RegModelToUser(RegModel regModel)
        {
            User user = new User()
            {
                UserName = regModel.UserName,
                Email = regModel.Email,
                Password = regModel.Password
            };
            return user;
        }

        public UserDTO UserToUserDTO(User user)
        {
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                RoleId = user.RoleId,
                TotalPrice = user.Cart?.TotalPrice ?? 0
            };
            return userDTO;
        }

        public ProductDTO ProductToProductDTO(Product product)
        {
            ProductDTO productDTO = new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };
            return productDTO;
        }

        public Product ProductDTOToProduct(ProductDTO productDTO)
        {
            Product product = new Product()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                ImageUrl= productDTO.ImageUrl
            };
            return product;
        }

        public CartDTO CartToCartDTO(Cart cart)
        {
            CartDTO cartDTO = new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
            };
            return cartDTO;
        }

        public CartItemDTO CartItemToCartItemDTO(CartItem cartItem)
        {
            CartItemDTO cartItemDTO = new CartItemDTO()
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
            return cartItemDTO;
        }

        public CartItem CartItemDTOToCartItem(CartItemDTO cartItemDTO)
        {
            CartItem cartItem = new CartItem()
            {
                Id = cartItemDTO.Id,
                CartId = cartItemDTO.CartId,
                ProductId = cartItemDTO.ProductId,
                Quantity = cartItemDTO.Quantity
            };
            return cartItem;
        }
    }
}
