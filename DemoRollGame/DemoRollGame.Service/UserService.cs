using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.DbCore.UoW;
using DemoRollGame.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoRollGame.Service
{
    public class UserService
    {
        private IGenericRepository<User> _userRepository;
        private IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User>();
        }
        public List<User> GetUsers()
        {
            return _userRepository.GetAll().ToList();
        }
       
        public User Login(string username , string password)
        {
            var user=_userRepository.GetAll().Where(x=>x.Password==password && x.UserName==username).FirstOrDefault();
            return user;
        }
    }
}
