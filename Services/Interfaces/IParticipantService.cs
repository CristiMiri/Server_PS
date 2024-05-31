using Server.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Domain.Model;

namespace Server.Services.Interfaces
{
    internal interface IParticipantService
    {
        bool CreateParticipant(ParticipantDTO participantDTO);
        bool UpdateParticipant(ParticipantDTO participantDTO);
        bool DeleteParticipant(int id);

        ParticipantDTO GetParticipant(int id);
        List<ParticipantDTO> GetAll();
        List<ParticipantDTO> GetParticipantsbySection(Section section);
        int GetLastParticipantId();
    }
}
