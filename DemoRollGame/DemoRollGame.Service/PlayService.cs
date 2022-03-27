using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.DbCore.UoW;
using DemoRollGame.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DemoRollGame.Service
{
    public class PlayService
    {
        private IGenericRepository<Match> _matchRepository;
        private IGenericRepository<UserMatch> _matchUserRepository;
        private IGenericRepository<User> _userRepository;
        private IUnitOfWork _unitOfWork;
        public PlayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _matchRepository = _unitOfWork.GetRepository<Match>();
            _matchUserRepository = _unitOfWork.GetRepository<UserMatch>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        /// <returns>The random number that user generated</returns>
        public int JoinToGame(int userId)
        {
            var user = _userRepository.FindBy(x => x.Id == userId).SingleOrDefault();
            if (!user.IsAvailable.GetValueOrDefault())
                return 0;

            Random rnd = new Random(DateTime.Now.Millisecond);
            int randomNumber = rnd.Next(1, 100);
            var currentMatch = GetCurrentMatch();
            if (currentMatch == null)
                throw new Exception();
            var matchUser = new UserMatch
            {
                Id = default,
                IsWinner = false,
                UserId = userId,
                JoinedAt = DateTime.Now,
                MatchId = currentMatch.Id,
                Roll = randomNumber,
            };
            user.IsAvailable = false;

            _userRepository.Update(user);
            _matchUserRepository.Add(matchUser);
            _unitOfWork.Commit();
            return randomNumber;

        }

        public int GetUserRoll(int userId)
        {
            var user = _userRepository.FindBy(x => x.Id == userId).SingleOrDefault();
            if (user == null)
                return 0;
            var currentMatch = GetCurrentMatch();
            if (currentMatch == null)
                return 0;

            var getRoll = _matchUserRepository.GetAll().Where(x => x.UserId == userId && x.MatchId == currentMatch.Id).FirstOrDefault();
            if (getRoll == null)
                return 0;

            return getRoll.Roll;

        }
        public void DecideToWinner()
        {
            var currentMatch = GetCurrentMatch();
            if (currentMatch == null)
                return;

            var players = _matchUserRepository.GetAll().Include(x => x.User).Where(x => x.MatchId == currentMatch.Id).ToList();
            var sortedPlayers = players.OrderByDescending(x => x.Roll);

            var winner = sortedPlayers.First();
            winner.IsWinner = true;
            _matchUserRepository.Update(winner);
            foreach (var player in players)
            {
                player.User.IsAvailable = true;
                _matchUserRepository.Update(player);
            }
            currentMatch.WinnerRoll = winner.Roll;
            currentMatch.WinnerName = winner.User.UserName;
            _unitOfWork.Commit();
        }
        private Match GetCurrentMatch()
        {
            var currentMatch = _matchRepository.GetAll().Where(x => x.IsComplated == false).FirstOrDefault();
            return currentMatch;
        }
        public Match GetMatchById(int id)
        {
            return _matchRepository.FindBy(x => x.Id == id).SingleOrDefault();
        }

    }
}
