using EventList.Application.Exceptions;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using EventList.Application.JWT;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventList.Infrastructure.CQRS.Commands
{
    public class UserCommands(IUnitOfWork unitOfWork, TokenProvider tokenProvider, IPasswordService passwordService)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly TokenProvider tokenProvider = tokenProvider;
        private readonly IPasswordService passwordService = passwordService;
        
        public async Task ToggleAdmin(User user)
        {
            if (await unitOfWork.Users.GetUserByGuid(user.UserId) == null)
                throw new NotFoundException("User not found");

            unitOfWork.Users.ToggleAdmin(user);
            await unitOfWork.SaveAsync();
        }   
        public async Task UpdateUserPfp(User user, string fileName)
        {
            if (await unitOfWork.Users.GetUserByGuid(user.UserId) == null)
                throw new NotFoundException("User not found");
            
            unitOfWork.Users.UpdateUserPfp(user, fileName);
            await unitOfWork.SaveAsync();
        }
        public async Task RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            if (await unitOfWork.Events.GetEventById(EventId) == null 
                || await unitOfWork.Users.GetUserByGuid(UserId) == null)
                throw new NotFoundException("User or event has not been found");

            unitOfWork.Users.EnrollUserInEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task RetractUserFromEvent(Guid EventId, Guid UserId)
        {
            if (await unitOfWork.Events.GetEventById(EventId) == null 
                || await unitOfWork.Users.GetUserByGuid(UserId) == null)
                throw new NotFoundException("User or event has not been found");

            unitOfWork.Users.RetractUserFromEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task<RefreshToken> GetRefreshToken(string accessToken)
        {
            JsonWebTokenHandler handler = new();
            var decodedToken = handler.ReadJsonWebToken(accessToken);
            return await unitOfWork.Users.GetRefreshToken(new Guid(decodedToken.Subject));
        }
        public async Task<string> Login(string Email, string Password)
        { 
            User user = await unitOfWork.Users.GetUserByEmail(Email)
                ?? throw new NotFoundException("User not found");

            if (!passwordService.VerifyPassword(Password, user))
                throw new UnauthorizedException("Invalid password");

            string token = tokenProvider.Create(user);
      
            if (await unitOfWork.Users.GetRefreshToken(user.UserId) == null)
            {
                unitOfWork.Users.CreateRefreshToken(user.UserId);
                await unitOfWork.SaveAsync();
            }
            return token;
        }
        public async Task AddUser(User user)
        {

            if (await unitOfWork.Users.GetUserByGuid(user.UserId) != null)
                throw new AlreadyExistsException("This user already exists!");

            user._Password = passwordService.Encrypt(user._Password);
            unitOfWork.Users.AddUser(user);
            await unitOfWork.SaveAsync();
        }
        public async Task<string> RefreshToken(string token)
        {
            JsonWebTokenHandler handler = new();
            var decodedToken = handler.ReadJsonWebToken(token);
            if (decodedToken.ValidTo > DateTime.UtcNow) return token;

            RefreshToken? refreshToken = await unitOfWork.Users.GetRefreshToken(new Guid(decodedToken.Subject))
                ?? throw new NotFoundException("Refresh token not found");

            if (refreshToken.Expiration <= DateTime.UtcNow)
                throw new BadRequestException("Refresh token has expired; log in please");

            return tokenProvider.Create(await unitOfWork.Users.GetUserByGuid(refreshToken.UserId));
        }

        public async Task ClearRefreshToken(RefreshToken refreshToken)
        {
            unitOfWork.Users.ClearRefreshToken(refreshToken);
            await unitOfWork.SaveAsync();
        }
    }
}
