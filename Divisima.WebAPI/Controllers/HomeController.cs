using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Divisima.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        IRepository<Brand> repoBrand;
        IRepository<Admin> repoAdmin;
        public HomeController(IRepository<Brand> _repoBrand, IRepository<Admin> _repoAdmin)
        {
            repoBrand = _repoBrand;
            repoAdmin = _repoAdmin;
        }
        //[HttpGet]
        //public string GetDate()
        //{
        //    return DateTime.Now.ToString();
        //}

        [HttpGet]
        public IEnumerable<Brand> GetBrands()
        {
            return repoBrand.GetAll().OrderByDescending(o => o.ID);
        }

        [HttpGet("{id}")]
        public Brand GetBrand(int id)
        {
            return repoBrand.GetBy(x => x.ID == id);
        }
        /// <summary>
        /// Burası Marka Ekleme Metodu falan filan.....
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Add(Brand brand)
        {
            try
            {
                await repoBrand.Add(brand);
                return brand.Name + " başarıyla eklendi...";
            }
            catch (Exception ex)
            {
                return "marka eklenemedi, hata açıklaması:" + ex.Message;
            }
        }

        [HttpDelete]
        [Route("/api/markasil/{id}")]
        public async Task<string> Remove(int id)
        {
            try
            {
                Brand brand = repoBrand.GetBy(x => x.ID == id);
                await repoBrand.Delete(brand);
                return brand.Name + " başarıyla silindi...";
            }
            catch (Exception ex)
            {
                return "marka silinemedi, hata açıklaması:" + ex.Message;
            }
        }

        [HttpPut]
        public async Task<string> Update(Brand brand)
        {
            try
            {
                await repoBrand.Update(brand);
                return brand.Name + " başarıyla güncellendi...";
            }
            catch (Exception ex)
            {
                return "marka güncellenemedi, hata açıklaması:" + ex.Message;
            }
        }

        [AllowAnonymous, Route("/api/login"), HttpGet]
        public string Login(string username, string password)
        {
            string md5Password = getMD5(password);
            Admin admin = repoAdmin.GetBy(x => x.UserName == username && x.Password == md5Password) ?? null;
            if (admin != null)
            {
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.PrimarySid,admin.ID.ToString()),
                    new Claim(ClaimTypes.Name,admin.Name+" "+admin.Surname)
                };
                string signinKey = "benimözelkeybilgisi";
                SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(signinKey));
                SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken jwtSecurityToken = new(
                    issuer: "http://localhost:5256",//token sağlayıcı url
                    audience: "n11",//kimliği kullanacak olan firma veya uygulmanın adı
                    claims: claims,
                    expires: DateTime.Now.AddDays(10),//token geçerlilik süresi
                    notBefore: DateTime.Now,//geçerliliği ne zaman başlasın
                    signingCredentials: signingCredentials
                    );
                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            else return "Kullanıcı adı veya şifre hatalı";
        }

        public static string getMD5(string _text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(_text));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }


}