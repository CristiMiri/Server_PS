
using Server.Domain.DTO;
using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Repositories
{
    internal class ParticipantRepository
    {
        private Repository repository;
        private UserRepository userRepository;
        private PresentationRepository presentationRepository;
        private Participant_PrezentareRepository participant_PrezentareRepository;

        public ParticipantRepository()
        {
            repository = Repository.Instance;
            userRepository = new UserRepository();
            presentationRepository = new PresentationRepository();
            participant_PrezentareRepository = new Participant_PrezentareRepository(this, presentationRepository);
        }

        //Utility methods
        private static ParticipantDTO RowToParticipant(DataRow row)
        {
            return new ParticipantDTO
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Email = row["email"].ToString(),
                Phone = row["phone"].ToString(),
                CNP = row["cnp"].ToString(),
                PdfFilePath = row["pdf_file_path"].ToString(),
                PhotoFilePath = row["photo_file_path"].ToString(),
            };
        }


        //CRUD methods
        public bool CreateParticipant(ParticipantDTO participant)
        {
            // Constructing SQL statement 
            string nonQuery = $"INSERT INTO participant (name, email, phone, cnp, pdf_file_path, photo_file_path) VALUES ('" +
                $"{participant.Name}', '" +
                $"{participant.Email}', '" +
                $"{participant.Phone}', '" +
                $"{participant.CNP}', '" +
                $"{participant.PdfFilePath}', '" +
                $"{participant.PhotoFilePath}')";
            // Execute the query
            return repository.ExecuteNonQuery(nonQuery);
        }

        public ParticipantDTO? ReadParticipantById(int id)
        {
            // Constructing SQL statement
            string query = $"SELECT * FROM participant WHERE id = {id}";
            DataTable participantiTable = repository.ExecuteQuery(query);
            if (participantiTable.Rows.Count == 0)
            {
                return null;
            }
            return RowToParticipant(participantiTable.Rows[0]);
        }

        public List<ParticipantDTO>? ReadParticipants()
        {
            // Constructing SQL statement
            string query = "SELECT * FROM participant";
            DataTable participantiTable = repository.ExecuteQuery(query);
            if (participantiTable.Rows.Count == 0)
            {
                return null;
            }

            // Convert DataTable to List<ParticipantDTO>
            List<ParticipantDTO> participanti = new List<ParticipantDTO>();
            foreach (DataRow row in participantiTable.Rows)
            {
                participanti.Add(RowToParticipant(row));
            }
            return participanti;
        }

        public bool UpdateParticipant(ParticipantDTO participant)
        {
            // Constructing SQL statement
            string nonQuery = $"UPDATE participant SET " +
                $"name = '{participant.Name}', " +
                $"email = '{participant.Email}', " +
                $"phone = '{participant.Phone}', " +
                $"cnp = '{participant.CNP}', " +
                $"pdf_file_path = '{participant.PdfFilePath}', " +
                $"photo_file_path = '{participant.PhotoFilePath}' " +
                $"WHERE id = {participant.Id}";
            // Execute the query
            return repository.ExecuteNonQuery(nonQuery);
        }

        public bool DeleteParticipant(int id)
        {
            // Constructing SQL statement
            string nonQuery = $"DELETE FROM participant WHERE id = {id}";
            // Execute the query
            return repository.ExecuteNonQuery(nonQuery);
        }

        public bool UpsertParticipant(ParticipantDTO participant)
        {
            if (ReadParticipantById(participant.Id) == null)
            {
                return CreateParticipant(participant);
            }
            return UpdateParticipant(participant);
        }

        //Filter methods
        public List<ParticipantDTO>? GetParticipantsBySection(Section section)
        {
            List<PresentationDTO> presentations = presentationRepository.ReadPresentationsBySection(section);
            if (presentations == null)
            {
                return null;
            }
            List<ParticipantDTO> participants = new List<ParticipantDTO>();
            foreach (PresentationDTO presentation in presentations)
            {
                List<ParticipantDTO> presentationParticipants = participant_PrezentareRepository.ReadParticipantsByPresentation(presentation);
                if (presentationParticipants != null)
                {
                    participants.AddRange(presentationParticipants);
                }
            }
            return participants;
        }

        public int GetLastParticipantId()
        {
            // Constructing SQL statement
            string query = "SELECT MAX(id) FROM participant";
            DataTable participantiTable = repository.ExecuteQuery(query);
            if (participantiTable.Rows.Count == 0)
            {
                return 0;
            }
            return Convert.ToInt32(participantiTable.Rows[0][0]);
        }
    }
}
