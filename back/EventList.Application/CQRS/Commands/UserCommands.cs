using EventList.Application.Exceptions;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using EventList.Application.JWT;
using Microsoft.IdentityModel.JsonWebTokens;
using EventList.Domain.CommandData;

namespace EventList.Application.CQRS.Commands
{
    public class UserCommands(IUnitOfWork unitOfWork, TokenProvider tokenProvider, IPasswordService passwordService)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly TokenProvider tokenProvider = tokenProvider;
        private readonly IPasswordService passwordService = passwordService;

        public async Task ToggleAdmin(ToggleAdminCommandData data)
        {
            if (await unitOfWork.Users.GetUserByGuid(data.User.UserId) == null)
                throw new NotFoundException("User not found");

            unitOfWork.Users.ToggleAdmin(data.User);
            await unitOfWork.SaveAsync();
        }
        public async Task UpdateUserPfp(UpdateUserPfpCommandData data)
        {
            if (await unitOfWork.Users.GetUserByGuid(data.User.UserId) == null)
                throw new NotFoundException("User not found");

            unitOfWork.Users.UpdateUserPfp(data.User, data.FileName);
            await unitOfWork.SaveAsync();
        }
        public async Task RegisterUserInEvent(RegisterUserInEventCommandData data)
        {
            if (await unitOfWork.Events.GetEventById(data.EventId) == null
                || await unitOfWork.Users.GetUserByGuid(data.UserId) == null)
                throw new NotFoundException("User or event has not been found");

            unitOfWork.Users.EnrollUserInEvent(data.EventId, data.UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task RetractUserFromEvent(RetractUserFromEventCommandData data)
        {
            if (await unitOfWork.Events.GetEventById(data.EventId) == null
                || await unitOfWork.Users.GetUserByGuid(data.UserId) == null)
                throw new NotFoundException("User or event has not been found");

            unitOfWork.Users.RetractUserFromEvent(data.EventId, data.UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task<RefreshToken> GetRefreshToken(GetRefreshTokenCommandData data)
        {
            JsonWebTokenHandler handler = new();
            var decodedToken = handler.ReadJsonWebToken(data.AccessToken);
            return await unitOfWork.Users.GetRefreshToken(new Guid(decodedToken.Subject));
        }
        public async Task<string> Login(LoginCommandData data)
        {
            User user = await unitOfWork.Users.GetUserByEmail(data.Email)
                ?? throw new NotFoundException("User not found");

            if (!passwordService.VerifyPassword(data.Password, user))
                throw new UnauthorizedException("Invalid password");

            if (await unitOfWork.Users.GetRefreshToken(user.UserId) == null)
            {
                unitOfWork.Users.CreateRefreshToken(user.UserId);
                await unitOfWork.SaveAsync();
            }
            if ((await unitOfWork.Users.GetRefreshToken(user.UserId)).Expiration < DateTime.UtcNow)
                throw new UnauthorizedException("Refresh Token has expired; log in please");
            return tokenProvider.Create(user);
        }
        public async Task AddUser(AddUserCommandData data)
        {

            if (await unitOfWork.Users.GetUserByGuid(data.User.UserId) != null)
                throw new AlreadyExistsException("This user already exists!");

            data.User._Password = passwordService.Encrypt(data.User._Password);
            unitOfWork.Users.AddUser(data.User);
            await unitOfWork.SaveAsync();
        }
        public async Task<string> RefreshToken(RefreshTokenCommandData data)
        {
            JsonWebTokenHandler handler = new();
            var decodedToken = handler.ReadJsonWebToken(data.Token);
            if (decodedToken.ValidTo > DateTime.UtcNow) return data.Token;

            RefreshToken? refreshToken = await unitOfWork.Users.GetRefreshToken(new Guid(decodedToken.Subject))
                ?? throw new NotFoundException("Refresh token not found");

            if (refreshToken.Expiration <= DateTime.UtcNow)
                throw new UnauthorizedException("Refresh token has expired; log in please");

            return tokenProvider.Create(await unitOfWork.Users.GetUserByGuid(refreshToken.UserId));
        }

        public async Task ClearRefreshToken(ClearRefreshTokenCommandData data)
        {
            unitOfWork.Users.ClearRefreshToken(data.RefreshToken);
            await unitOfWork.SaveAsync();
        }
    }
}
