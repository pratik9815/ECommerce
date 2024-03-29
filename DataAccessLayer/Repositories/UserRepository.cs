﻿using DataAccessLayer.Common;
using DataAccessLayer.DataContext;
using DataAccessLayer.DTO.User;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public UserRepository(ApplicationDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<List<UserDTO>> GetAllAdminUsers()
        {

            var users = await _context.Users
                                            .Where(x => x.UserType == UserType.SuperAdmin || x.UserType == UserType.Admin)
                                            .OrderBy(x => x.FullName)
                                            .Select(x => new UserDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                                //CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName
                                            }).ToListAsync();
            return users;
        }
        public async Task<List<UserDTO>> GetSuperAdminUsers()
        {

            var users = await _context.Users
                                            .Where(x => x.UserType == UserType.SuperAdmin)
                                            .OrderBy(x => x.FullName)
                                            .Select(x => new UserDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                                //CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName
                                            }).ToListAsync();
            return users;
        }

        public async Task<List<UserDTO>> GetAdminUser()
        {
            var users = await _context.Users
                                            .Where(x =>  x.UserType == UserType.Admin)
                                            .OrderBy(x => x.FullName)
                                            .Select(x => new UserDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName
                                            }).ToListAsync();
            return users;
        }

        public async Task<List<UserDTO>> GetCustomer()
        {
            var users = await _context.Users
                                            .Where(x => x.UserType == UserType.Customer)
                                            .OrderBy(x => x.FullName)
                                            .Select(x => new UserDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                                CreatedBy = _context.Users.FirstOrDefault(a => a.Id == x.CreatedBy).FullName,
                                                CustomerId = x.CustomerId.ToString()
                                            }).ToListAsync();
            return users;
        }

        public async Task<UserDTO> GetAdminUsersById(string id)
        {
            var users = await _context.Users
                                            .Where(x => x.UserType == UserType.Admin && x.Id == id)
                                            .Select(x => new UserDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                            }).FirstOrDefaultAsync();
            return users;
        }

        public async Task<CustomerDTO> GetCustomerUsersById(string id)
        {
            var users = await _context.Users
                                            .Where(x => x.UserType == UserType.Customer && x.CustomerId == Guid.Parse(id))
                                            .Select(x => new CustomerDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                                CustomerId = x.CustomerId.ToString(),  
                                            }).FirstOrDefaultAsync();
            return users;
        }

        public async Task<UserDTO> GetSuperAdminUsersById(string id)
        {
            var users = await _context.Users
                                            .Where(x => x.UserType == UserType.SuperAdmin && x.Id == id)
                                            .Select(x => new UserDTO
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                UserName = x.UserName,
                                                Address = x.Address,
                                                DOB = x.DOB,
                                                Gender = x.Gender.GetEnumDisplayName(),
                                                UserType = x.UserType.GetEnumDisplayName(),
                                                Email = x.Email,
                                                PhoneNumber = x.PhoneNumber,
                                            }).FirstOrDefaultAsync();
            return users;
        }
    }  
    


}
