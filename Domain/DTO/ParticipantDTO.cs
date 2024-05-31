
using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.DTO
{
    internal class ParticipantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CNP { get; set; }
        public string PdfFilePath { get; set; }
        public string PhotoFilePath { get; set; }

        // Default constructor
        public ParticipantDTO() { }

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
    }
}
