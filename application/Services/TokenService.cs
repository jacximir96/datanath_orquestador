using application.Interfaces;
using domain.Entities;
using domain.Interfaces;


namespace application.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenAdapter _tokenAdapter;   
        private readonly IEncryptAdapter _encryptAdapter;
        private readonly IUserRepository<User> _userRepository;
        public TokenService(
            ITokenAdapter tokenAdapter, 
            IEncryptAdapter encryptAdapter, 
            IUserRepository<User> userRepository
            ) 
        {
            _tokenAdapter = tokenAdapter;
            _encryptAdapter = encryptAdapter;
            _userRepository = userRepository;
        }

        public GenericResponse GenerateToken(string user, string password)
        {
            string decryptedPassword=string.Empty;
            string token=string.Empty;
            GenericResponse response = new GenericResponse();   
            try
            {
                var _user =_userRepository.GetUser(user, password).Result;
                if (_user != null) 
                {
                    decryptedPassword = _encryptAdapter.Decrypt(_user.Password, Resources.GeneralKeyToEncrypt);
                    if (_user.Name == user && password == decryptedPassword) 
                    {
                        token = _tokenAdapter.GenerateToken(user);
                        response.Error = false;
                        response.token = token;  
                    }

                    return response;    
                } 
                response.Error=true;
                response.Code= 400;
                return response;    
                
            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }
            return response;
        }
    }
}
