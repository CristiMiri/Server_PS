
using Server.Domain.DTO;
using Server.Domain.Model;
using Server.Repositories;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class ParticipantService :IParticipantService
    {
        private ParticipantRepository _participantRepository;
        public ParticipantService()
        { 
            _participantRepository = new ParticipantRepository();
        }

        public bool CreateParticipant(ParticipantDTO participantDTO)
        {
            return _participantRepository.CreateParticipant(participantDTO);
        }

        public bool DeleteParticipant(int id)
        {
            return _participantRepository.DeleteParticipant(id);
        }

        public List<ParticipantDTO> GetAll()
        {
            return _participantRepository.ReadParticipants();
        }

        public int GetLastParticipantId()
        {
            return _participantRepository.GetLastParticipantId();
        }

        public ParticipantDTO GetParticipant(int id)
        {
            return _participantRepository.ReadParticipantById(id);
        }

        public List<ParticipantDTO> GetParticipantsbySection(Section section)
        {
            return _participantRepository.GetParticipantsBySection(section);
        }

        public bool UpdateParticipant(ParticipantDTO participantDTO)
        {
            return _participantRepository.UpdateParticipant(participantDTO);
        }
    }
}
