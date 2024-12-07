﻿using AutoMapper;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using EventList.Domain.QueryData;

namespace EventList.Infrastructure.CQRS.Queries
{
    public class UserQueries(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        public List<User> GetAllUsers()
        {
            return [.. unitOfWork.Users.GetUsers()];
        }
        public async Task<User?> GetUserByGuid(GetUserByGuidQueryData data)
        {
            return await unitOfWork.Users.GetUserByGuid(data.Id);
        }
        public List<User> GetUsersForThisEvent(GetUsersForThisEventQueryData data)
        {
            return unitOfWork.Users.GetUsersForThisEvent(
                unitOfWork.Users.GetEUForEvent(data.EventId));
        }

    }
}
