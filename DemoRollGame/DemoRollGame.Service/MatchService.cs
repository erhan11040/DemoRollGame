using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.DbCore.UoW;
using DemoRollGame.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoRollGame.Service
{
    public class MatchService
    {
        private IGenericRepository<Match> _matchRepository;
        private IUnitOfWork _unitOfWork;
        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _matchRepository = _unitOfWork.GetRepository<Match>();
        }
        public List<Match> GetAllMatches()
        {
            return _matchRepository.GetAll().ToList();
        }
        public Match GetMatchById(int id)
        {
            return _matchRepository.FindBy(x=>x.Id==id).SingleOrDefault();
        }
        public Match CreateMatch()
        {
            var now = DateTime.Now;
            var match = new Match()
            {
                ExpiresAt=now.AddMinutes(10),
                Id=default,
                IsComplated=false,
                Name=now.ToString(),
                StartedAt=now,
            };
            _matchRepository.Add(match);
            _unitOfWork.Commit();

            return match;
        }
        public Match ComplateMatch(int id)
        {
            var currentMatch = _matchRepository.FindBy(x=>x.Id== id).SingleOrDefault();
            if (currentMatch == null)
                return null;
            currentMatch.IsComplated = true;
            _matchRepository.Update(currentMatch);
            _unitOfWork.Commit();
            return currentMatch;
        }
        public Match GetCurrentMatch()
        {
            var currentMatch = _matchRepository.GetAll().Where(x => x.IsComplated == false).FirstOrDefault();
            return currentMatch;
        }
       
    }
}
