using CanaryDeliveries.Backoffice.Api.Auth;
using CanaryDeliveries.Backoffice.Api.Security;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Services
{
    public class LoginService
    {
        private readonly BackofficeUserRepository backofficeUserRepository;
        private readonly CryptoServiceProvider cryptoServiceProvider;
        private readonly TokenHandler tokenHandler;

        public LoginService(
            BackofficeUserRepository backofficeUserRepository,
            CryptoServiceProvider cryptoServiceProvider, 
            TokenHandler tokenHandler)
        {
            this.backofficeUserRepository = backofficeUserRepository;
            this.cryptoServiceProvider = cryptoServiceProvider;
            this.tokenHandler = tokenHandler;
        }

        public virtual Either<LoginError, Token> Authenticate(LoginRequest request)
        {
            return
                from backofficeUser in SearchBackofficeUser(request.Email)
                from _ in ValidateCredentials(backofficeUser, request.Password)
                from token in CreateToken(backofficeUser)
                select token;
            
            Either<LoginError, BackofficeUser> SearchBackofficeUser(string email)
            {
                return backofficeUserRepository.SearchBy(email)
                    .ToEither(() => LoginError.UserNotFound);
            }
            
            Either<LoginError, Unit> ValidateCredentials(
                BackofficeUser user, 
                string passwordIntent)
            {
                var passwordIntentHash = cryptoServiceProvider.ComputeHash(passwordIntent);
                if(passwordIntentHash.Value == user.Password)
                {
                    return unit;
                }
                return LoginError.InvalidCredentials;
            }
            
            Either<LoginError, Token> CreateToken(BackofficeUser user)
            {
                return tokenHandler.Create(user);
            }
        }
        
        public sealed class LoginRequest
        {
            public string Email { get; }
            public string Password { get; }

            public LoginRequest(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }
    }

    public enum LoginError
    {
        UserNotFound,
        InvalidCredentials
    }
}