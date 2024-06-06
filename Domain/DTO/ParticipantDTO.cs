using Server.Domain.Model;
using System.Collections.Generic;

namespace Server.Domain.DTO
{
    internal class ParticipantDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string CNP { get; private set; }
        public string PdfFilePath { get; private set; }
        public string PhotoFilePath { get; private set; }

        // Default constructor made private to enforce the use of the builder
        private ParticipantDTO() { }

        // Constructor to initialize the DTO from a Participant entity
        public ParticipantDTO(Participant participant)
        {
            Id = participant.Id;
            Name = participant.Name;
            Email = participant.Email;
            Phone = participant.Phone;
            CNP = participant.CNP;
            PdfFilePath = participant.PdfFilePath;
            PhotoFilePath = participant.PhotoFilePath;
        }

        public override string ToString()
        {
            return $"ParticipantDTO with id {Id}, name {Name}, email {Email}, phone {Phone}, CNP {CNP}, PDF file path {PdfFilePath}, photo file path {PhotoFilePath}.";
        }

        // Converts a list of Participant entities to a list of ParticipantDTOs
        public static List<ParticipantDTO> FromParticipantList(List<Participant> participants)
        {
            var participantDTOs = new List<ParticipantDTO>();
            foreach (var participant in participants)
            {
                participantDTOs.Add(new ParticipantDTO(participant));
            }
            return participantDTOs;
        }

        // Builder class
        public class Builder
        {
            private readonly ParticipantDTO _participantDTO;

            public Builder()
            {
                _participantDTO = new ParticipantDTO();
            }

            public Builder SetId(int id)
            {
                _participantDTO.Id = id;
                return this;
            }

            public Builder SetName(string name)
            {
                _participantDTO.Name = name;
                return this;
            }

            public Builder SetEmail(string email)
            {
                _participantDTO.Email = email;
                return this;
            }

            public Builder SetPhone(string phone)
            {
                _participantDTO.Phone = phone;
                return this;
            }

            public Builder SetCNP(string cnp)
            {
                _participantDTO.CNP = cnp;
                return this;
            }

            public Builder SetPdfFilePath(string pdfFilePath)
            {
                _participantDTO.PdfFilePath = pdfFilePath;
                return this;
            }

            public Builder SetPhotoFilePath(string photoFilePath)
            {
                _participantDTO.PhotoFilePath = photoFilePath;
                return this;
            }

            public ParticipantDTO Build()
            {
                return _participantDTO;
            }
        }
    }
}
