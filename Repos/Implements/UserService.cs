using BOs;
using BOs.DTOS;
using DAOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repos.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;  // Inject configuration for JWT settings
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserResponse registerAccount(UserRegisterRequest request)
        {
            // Check if user with the same email already exists
            var existingUser = _unitOfWork.UserRepository.Get(filter: u => u.Email == request.Email);
            if (existingUser.ToList().Count() > 0)
            {
                throw new Exception("User with this email already exists.");
            }

            // Hash the password (you can use a library like BCrypt or another secure method)
            var hashedPassword = HashPassword(request.Password);

            // Create a new user
            var user = new User
            {
                RoleID = request.RoleID ,
                Password = hashedPassword,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                Status = "Active"
            };

            // Add the new user to the database
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();

            // Generate JWT Token for the new user
            var token = GenerateJwtToken(user);
            var role = _unitOfWork.RoleRepository.GetByID(user.RoleID);

            // Return the user along with the token
            return new UserResponse(token, user.FullName, user.Email, role.Name);
        }

        private string HashPassword(string password)
        {
            // Generate a salt and hash the password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string providedPassword, string storedHashedPassword)
        {
            // Use BCrypt to verify the password against the stored hash
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedHashedPassword);
        }


        private string GenerateJwtToken(User user)
        {
            var role = _unitOfWork.RoleRepository.GetByID(user.RoleID);
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            // Define the token claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, role.Name.ToUpper())
            };

            // Generate the JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"])),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the JWT token
            return tokenHandler.WriteToken(token);
        }

        // Method to generate a strong random secret key
        public static string GenerateRandomKey(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                randomNumberGenerator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public UserResponse Login(UserLoginRequest request)
        {
            // Retrieve the user by email
            var user = _unitOfWork.UserRepository.Get(filter: u => u.Email == request.Email).FirstOrDefault();

            // Check if the user exists
            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            // Verify the password (ensure you hash the provided password before comparison)
            if (!VerifyPassword(request.Password, user.Password))
            {
                throw new Exception("Invalid email or password.");
            }

            // Generate JWT Token for the user
            var token = GenerateJwtToken(user);
            var role = _unitOfWork.RoleRepository.GetByID(user.RoleID);

            // Return the user along with the token
            return new UserResponse(token, user.FullName, user.Email, role.Name);
        }

        public List<User> GetUsers()
        {
            return _unitOfWork.UserRepository.Get(includeProperties: "Role").ToList();
        }

        public User GetCurrentUser()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("nameid");

                if (userIdClaim != null)
                {
                    return _unitOfWork.UserRepository.Get( filter: u => u.UserID == Guid.Parse(userIdClaim.Value), includeProperties:"Role").FirstOrDefault();
                }
            }

            throw new Exception("User ID not found.");
        }
    }
}
