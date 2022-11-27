using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        public IConfiguration _configuration { get; }
        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dataContext = dataContext;

        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(U => U.Username.ToLower() == username.ToLower());

            if (user != null && VerifyPasswardHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = true;
                response.Message = "You have successfully logged in!";
                response.Data = CreateToken(user);
            }
            else
            {
                response.Success = false;
                response.Message = "The details entered do not match our records!";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            if (await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "Username is unavailable";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            //Id is generated after save
            response.Data = user.Id;
            return response;

        }

        public async Task<bool> UserExists(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.Username.ToLower() == username);

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Checks if password entered matches password hashed and salted
        private bool VerifyPasswardHash(string passsword, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passsword));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user){
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}