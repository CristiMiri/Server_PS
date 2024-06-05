using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    internal interface IStatisticsService
    {
        Dictionary<string, int> GetNumberOfParticipantsByConference();
        Dictionary<string, int> GetNumberOfParticipantsBySection();
        Dictionary<string, int> GetNumberOfPresentationsByAuthor();
        Dictionary<DateTime, int> GetNumberOfPresentationsPerDay();
    }
}
