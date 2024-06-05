using Server.Repositories;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private StatisticsRepository _statisticsRepository;
        public StatisticsService()
        {
            _statisticsRepository = new StatisticsRepository();
        }

        public Dictionary<string, int> GetNumberOfParticipantsByConference()
        {
            return _statisticsRepository.GetNumberOfParticipantsByConference();
        }
        public Dictionary<string, int> GetNumberOfParticipantsBySection()
        {
            return _statisticsRepository.GetNumberOfParticipantsBySection();
        }
        public Dictionary<string, int> GetNumberOfPresentationsByAuthor()
        {
            return _statisticsRepository.GetNumberOfPresentationsByAuthor();
        }
        public Dictionary<DateTime, int> GetNumberOfPresentationsPerDay()
        {
            return _statisticsRepository.GetNumberOfPresentationsPerDay();
        }
    }
}
