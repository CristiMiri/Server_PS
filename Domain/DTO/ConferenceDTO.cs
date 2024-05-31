using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Domain.DTO
{
    internal class ConferenceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }

        // Default constructor
        public ConferenceDTO() { }

        // Constructor to initialize the DTO from a Conference entity
        public ConferenceDTO(Conference conference)
        {
            Id = conference.Id;
            Title = conference.Title;
            Location = conference.Location;
            Date = conference.Date;
        }

        public override string ToString()
        {
            return $"ConferenceDTO with id {Id}, title {Title}, location {Location}, and date {Date}.";
        }

        // Converts a list of Conference entities to a list of ConferenceDTOs
        public static List<ConferenceDTO> FromConferenceList(List<Conference> conferences)
        {
            var conferenceDTOs = new List<ConferenceDTO>();
            foreach (var conference in conferences)
            {
                conferenceDTOs.Add(new ConferenceDTO(conference));
            }
            return conferenceDTOs;
        }
    }
}
