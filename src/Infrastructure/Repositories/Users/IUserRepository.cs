﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Users
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUserById(int id);
        public Task<User> GetUserById(long telegramId);
        public Task<User> CreateOrModifyUser(User user);
        public Task<User> DeleteUser(User user);
    }
}
