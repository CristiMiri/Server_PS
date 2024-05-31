
using Server.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Repositories
{
    internal class Participant_PrezentareRepository
    {
        private Repository repository;
        private ParticipantRepository participantRepository;
        private PresentationRepository prezentareRepository;

        public Participant_PrezentareRepository(ParticipantRepository participantRepository, PresentationRepository prezentareRepository)
        {
            this.repository = Repository.Instance;
            this.participantRepository = participantRepository;
            this.prezentareRepository = prezentareRepository;
        }

        // Create
        public bool CreateRelation(ParticipantDTO participant, PresentationDTO prezentare)
        {
            // Constructing SQL statement
            string nonQuery = $"INSERT INTO presentation_Participant (id_presentation, id_participant) VALUES (" +
                              $"{prezentare.Id}, " +
                              $"{participant.Id})";
            // Execute the query
            return repository.ExecuteNonQuery(nonQuery);
        }

        // Read Participants by PresentationDTO
        public List<ParticipantDTO> ReadParticipantsByPresentation(PresentationDTO prezentare)
        {
            // Constructing SQL statement
            string query = $"SELECT * FROM presentation_Participant WHERE id_presentation = {prezentare.Id}";
            DataTable dataTable = repository.ExecuteQuery(query);
            List<ParticipantDTO> participants = new List<ParticipantDTO>();

            // Convert DataTable to List<ParticipantDTO>
            foreach (DataRow row in dataTable.Rows)
            {
                int idParticipant = Convert.ToInt32(row["id_participant"]);
                ParticipantDTO participant = participantRepository.ReadParticipantById(idParticipant);
                participants.Add(participant);
            }
            return participants;
        }

        // Read Presentations by ParticipantDTO
        public List<PresentationDTO> ReadPresentationsByParticipant(ParticipantDTO participant)
        {
            // Constructing SQL statement
            string query = $"SELECT * FROM presentation_Participant WHERE id_participant = {participant.Id}";
            DataTable dataTable = repository.ExecuteQuery(query);
            List<PresentationDTO> presentations = new List<PresentationDTO>();

            // Convert DataTable to List<PresentationDTO>
            foreach (DataRow row in dataTable.Rows)
            {
                int idPresentation = Convert.ToInt32(row["id_presentation"]);
                PresentationDTO presentation = prezentareRepository.ReadPresentationById(idPresentation);
                presentations.Add(presentation);
            }
            return presentations;
        }

        // Delete
        public bool DeleteRelation(ParticipantDTO participant, PresentationDTO prezentare)
        {
            // Constructing SQL statement
            string nonQuery = $"DELETE FROM presentation_Participant WHERE id_participant = {participant.Id} AND id_presentation = {prezentare.Id}";
            return repository.ExecuteNonQuery(nonQuery);
        }

        public bool CreateParticipantPresentation(int idParticipant, object idPresentation)
        {
            string nonQuery = $"INSERT INTO presentation_Participant (id_participant, id_presentation) VALUES ({idParticipant}, {idPresentation})";
            return repository.ExecuteNonQuery(nonQuery);
        }
    }
}
