using EventList.Application.Exceptions;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EventList.Application.JWT;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventList.Infrastructure.CQRS.Commands
{
    public class UserCommands(IUnitOfWork unitOfWork, ILoginUser loginUser, TokenProvider tokenProvider)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ILoginUser loginUser = loginUser;
        private readonly TokenProvider tokenProvider = tokenProvider;
        
        internal static string Encrypt(string value)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLower();
        }
        public async Task ToggleAdmin(User user)
        {
            if (await unitOfWork.Users.GetUserByGuid(user.UserId) != null)
            {
                unitOfWork.Users.ToggleAdmin(user);
                await unitOfWork.SaveAsync();
            }
            else throw new NotFoundException("User not found");
        }
        public async Task UpdateUserPfp(User user, string fileName)
        {
            if (await unitOfWork.Users.GetUserByGuid(user.UserId) != null)
            {
                unitOfWork.Users.UpdateUserPfp(user, fileName);
                await unitOfWork.SaveAsync();
            }
            else throw new NotFoundException("User not found");
        }
        public async Task RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            if (await unitOfWork.Events.GetEventById(EventId) == null || await unitOfWork.Users.GetUserByGuid(UserId) == null)
                throw new NotFoundException("User or event has not been found");
            else
            {
                unitOfWork.Users.EnrollUserInEvent(EventId, UserId);
                await unitOfWork.SaveAsync();
            }
        }
        public async Task RetractUserFromEvent(Guid EventId, Guid UserId)
        {
            if (await unitOfWork.Events.GetEventById(EventId) == null || await unitOfWork.Users.GetUserByGuid(UserId) == null)
                throw new NotFoundException("User or event has not been found");
            else
            {
                unitOfWork.Users.RetractUserFromEvent(EventId, UserId);
                await unitOfWork.SaveAsync();
            }
        }
        public async Task<string> Login(string Email, string Password)
        {
            try
            {
                string token = loginUser.Handle(Email, Password);
                User user = await unitOfWork.Users.GetUserByEmail(Email);
                if (GetRefreshToken(user.UserId) != null)
                {
                    unitOfWork.Users.UpdateRefreshToken(GetRefreshToken(user.UserId), token);
                }
                else
                {
                    unitOfWork.Users.SaveRefreshToken(token, user.UserId);
                }
                await unitOfWork.SaveAsync();
                return token;

            }
            catch (UnauthorizedException)
            {
                throw new UnauthorizedException();
            }
        }
        public async Task AddUser(User user, ModelStateDictionary state)
        {
            if (await unitOfWork.Users.GetUserByGuid(user.UserId) == null)
            {
                if (state.IsValid)
                    user._Password = Encrypt(user._Password);
                unitOfWork.Users.AddUser(user);
                await unitOfWork.SaveAsync();
            }
            else throw new AlreadyExistsException("This user already exists!");
        }
        public RefreshToken? GetRefreshToken(Guid userId)
        {
            return unitOfWork.Users.GetRefreshTokenByGuid(userId).Result;
        }
        public RefreshToken? GetRefreshTokenByToken(string token)
        {
            return unitOfWork.Users.GetRefreshTokenByToken(token).Result;
        }
        public async Task<string> RefreshToken(string token)
        {
            JsonWebTokenHandler handler = new();
            var decodedToken = handler.ReadJsonWebToken(token);
            if (decodedToken.ValidTo > DateTime.UtcNow) return token;
            else
            {
                RefreshToken? refreshToken = unitOfWork.Users.GetRefreshTokenByGuid(new Guid(decodedToken.Subject)).Result;
                if (refreshToken == null) throw new NotFoundException("Refresh token not found");
                else
                {
                    if (refreshToken.Expiration > DateTime.UtcNow)
                    {
                        string newToken = tokenProvider.Create(await unitOfWork.Users.GetUserByGuid(refreshToken.UserId));

                        await unitOfWork.Users.UpdateRefreshToken(refreshToken, newToken);
                        await unitOfWork.SaveAsync();
                        return newToken;
                    }
                    else throw new BadRequestException("Refresh token has expired; log in please");
                }
            }
        }

        public async Task ClearRefreshToken(RefreshToken refreshToken)
        {
            unitOfWork.Users.ClearRefreshToken(refreshToken);
            await unitOfWork.SaveAsync();
        }
    }
}
