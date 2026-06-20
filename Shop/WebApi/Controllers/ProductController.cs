using Database.Entities;
using Database.Repositories;
using DTO.DTOEntities;
using DTO.Entities;
using DTO.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _rep;

        private readonly IAppMappingProfile _appMappingProfile;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IRepository<Product> rep, IAppMappingProfile appMappingProfile, IWebHostEnvironment webHostEnvironment)
        {
            _rep = rep;
            _appMappingProfile = appMappingProfile;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("getall")]
        public ActionResult<List<ProductDTO>> GetAll()
        {
            var res = _rep.GetAll().Select(x => _appMappingProfile.ProductToProductDTO(x)).ToList();
            return Ok(res);
        }

        [HttpGet("getbyid/{id:int}")]
        public ActionResult<ProductDTO> GetById(int id)
        {
            return Ok(_appMappingProfile.ProductToProductDTO(_rep.GetByFunc(x => x.Id == id)!));
        }

        [HttpGet("getbytext")]
        public ActionResult<List<ProductDTO>> GetByText([FromQuery] string searchtext)
        {
            return Ok(_rep.GetAll().Where(x => x.Name.ToLower().Contains(searchtext.ToLower())).Select(x => _appMappingProfile.ProductToProductDTO(x)).ToList());
        }

        [HttpPost("addnew")]
        [Authorize(Roles = "Admin")]
        public ActionResult Add([FromBody] ProductDTO productDTO)
        {
            Product product = _appMappingProfile.ProductDTOToProduct(productDTO);
            _rep.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPost("saveimage")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Add([FromBody] string imageBase64)
        {
            if(imageBase64 == string.Empty)
            {
                return Ok(new ImageResponse { ImageUrl = "/images/no-image.png" });
            }
            var base64Data = imageBase64.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(base64Data);

            var contentType = imageBase64.Split(";")[0].Replace("data:", "").Split('/')[1];
            string fileName = $"{Guid.NewGuid()}.{contentType}";
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fullPath = Path.Combine(folderPath, fileName);

            await System.IO.File.WriteAllBytesAsync(fullPath, imageBytes);
            return Ok(new ImageResponse { ImageUrl = $"/images/products/{fileName}" });
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update([FromBody] ProductDTO productDTO)
        {
            Product product = _appMappingProfile.ProductDTOToProduct(productDTO);
            _rep.Update(product);
            return NoContent();
        }

        [HttpDelete("delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _rep.Delete(_rep.GetByFunc(x => x.Id == id)!);
            return NoContent();
        }
    }
}
