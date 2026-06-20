using Database.Entities;
using DTO.DTOEntities;
using DTO.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Profiles
{
    public interface IAppMappingProfile
    {
        public User RegModelToUser(RegModel regModel);

        public UserDTO UserToUserDTO(User user);

        public ProductDTO ProductToProductDTO(Product product);

        public Product ProductDTOToProduct(ProductDTO product);

        public CartDTO CartToCartDTO(Cart cart);

        public CartItemDTO CartItemToCartItemDTO(CartItem cartItem);

        public CartItem CartItemDTOToCartItem(CartItemDTO cartItemDTO);
    }
}
