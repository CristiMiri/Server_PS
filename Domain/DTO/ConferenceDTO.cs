using Server.Domain.Model;
using System;
using System.Collections.Generic;

namespace Server.Domain.DTO
{
    internal class ConferenceDTO
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Location { get; private set; }
        public DateTime Date { get; private set; }

        // Default constructor made private to enforce the use of the builder
        private ConferenceDTO() { }

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

        // Builder class
        public class Builder
        {
            private readonly ConferenceDTO _conferenceDTO;

            public Builder()
            {
                _conferenceDTO = new ConferenceDTO();
            }

            public Builder SetId(int id)
            {
                _conferenceDTO.Id = id;
                return this;
            }

            public Builder SetTitle(string title)
            {
                _conferenceDTO.Title = title;
                return this;
            }

            public Builder SetLocation(string location)
            {
                _conferenceDTO.Location = location;
                return this;
            }

            public Builder SetDate(DateTime date)
            {
                _conferenceDTO.Date = date;
                return this;
            }

            public ConferenceDTO Build()
            {
                return _conferenceDTO;
            }
        }
    }
}
