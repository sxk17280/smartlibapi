using Microsoft.Extensions.Options;
using SmartLibraryManager.Models;
using MongoDB.Driver;
using SmartLibraryManager.Common.Models;
using SmartLibraryManager.Common.IntitialData;
using SmartLibraryManager.ViewModels;
using System.Security.Claims;

namespace SmartLibraryManager.Services
{
    public class TokenService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly JWTService _jwtService;

        public TokenService(IOptions<DatabaseSettings> bookStoreDatabaseSettings, JWTService jwtService)
        {
            BookStoreData bookStoreData = new BookStoreData();

            var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>("Users");

            if (_userCollection.Find(_ => true).CountDocuments() == 0)
            {
                _userCollection.InsertMany(bookStoreData.GetUsers());
            }

            _jwtService = jwtService;
        }
        public async Task<User?> GetAsync(string id,string password)
        {
            return await _userCollection.Find(x => x.Email == id&&x.Password==password).FirstOrDefaultAsync();
        }
        public async Task<User?> CheckLogin(LoginVM loginVM)
        {
            return await _userCollection.Find(x => x.Email == loginVM.UserName && x.Password == loginVM.Password).FirstOrDefaultAsync();
        }
        private async Task<User> AuthenticateUser(LoginVM loginVM)
        {
            var model = await GetAsync(loginVM.UserName,loginVM.Password);

            if (model == null)
            {
                // authentication failed
                return null;
            }
            return model;
        }

        public async Task<object> UserLogin(LoginVM loginVM)
        {
            var user = await AuthenticateUser(loginVM);
            if (user != null)
            {
                var claims = GetUserClaims(user);
                var tokenstring = _jwtService.GenerateJSONWebToken(claims: claims);
                return new { Token = tokenstring,userDetails = user };
            }
            return "Unauthorized";
        }

        public List<Claim> GetUserClaims(User user)
        {
            var role = user.IsAdmin ? 1 : 0;
            List<Claim> claims = new List<Claim>()
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("Email", user.Email),
                    new Claim("RoleId", role.ToString()),
                    new Claim("isAdmin",role.ToString()),
                };
            return claims;
        }
    }

}
