using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.DTO
{
    internal class PresentationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Hour { get; set; }
        public Section Section { get; set; }
        public int IdConference { get; set; }
        public int IdAuthor { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
        public List<ParticipantDTO> Author { get; set; }

        // Default constructor
        public PresentationDTO() { }

        // Constructor to initialize the DTO from a Presentation entity
        public PresentationDTO(Presentation presentation)
        {
            Id = presentation.Id;
            Title = presentation.Title;
            Description = presentation.Description;
            Date = presentation.Date;
            Hour = presentation.Hour;
            Section = presentation.Section;
            IdConference = presentation.IdConference;
            IdAuthor = presentation.IdAuthor;
            Participants = ParticipantDTO.FromParticipantList(presentation.Participants);
            Author = ParticipantDTO.FromParticipantList(presentation.Author);
        }

        public override string ToString()
        {
            return $"PresentationDTO with id {Id} and title {Title}, description: {Description}, date: {Date}, hour: {Hour}, section: {Section}, conference id: {IdConference}, author id: {IdAuthor}";
        }

        // Converts a list of Presentation entities to a list of PresentationDTOs
        public static List<PresentationDTO> FromPresentationList(List<Presentation> presentations)
        {
            var presentationDTOs = new List<PresentationDTO>();
            foreach (var presentation in presentations)
            {
                presentationDTOs.Add(new PresentationDTO(presentation));
            }
            return presentationDTOs;
        }
    }
}
