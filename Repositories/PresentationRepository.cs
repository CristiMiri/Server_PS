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
    internal class PresentationRepository
    {
        private Repository repository;

        public PresentationRepository()
        {
            repository = Repository.Instance;
        }


        //Utility methods
        private PresentationDTO RowToPresentation(DataRow row)
        {
            return new PresentationDTO
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Description = row["description"].ToString(),
                Date = Convert.ToDateTime(row["date"]),
                Hour = TimeSpan.Parse(row["hour"].ToString()),
                Section = (Section)Enum.Parse(typeof(Section), row["section"].ToString()),
                IdConference = Convert.ToInt32(row["id_conference"]),
                IdAuthor = Convert.ToInt32(row["id_author"])
            };
        }



        //CRUD methods
        public bool CreatePresentation(PresentationDTO presentation)
        {
            // Safely format the date and time 
            string safeDate = presentation.Date.ToString("yyyy-MM-dd");
            string safeTime = presentation.Hour.ToString();

            string titleEscaped = presentation.Title.Replace("'", "''");
            string descriptionEscaped = presentation.Description.Replace("'", "''");

            string query = $"INSERT INTO presentation (title, description, date, hour, section, id_conference, id_author) " +
                $"VALUES ('{titleEscaped}', " +
                $"'{descriptionEscaped}', " +
                $"'{safeDate}', " +
                $"'{safeTime}', " +
                $"'{presentation.Section}', " +
                $"{presentation.IdConference}, " +
                $"{presentation.IdAuthor})";
            return repository.ExecuteNonQuery(query);
        }

        public PresentationDTO? ReadPresentationById(int id)
        {
            string query = $"SELECT * FROM presentation WHERE id = {id}";
            DataTable table = repository.ExecuteQuery(query);
            if (table.Rows.Count > 0)
            {
                return RowToPresentation(table.Rows[0]);
            }
            return null;
        }

        public List<PresentationDTO>? ReadPresentations()
        {
            string query = "SELECT * FROM presentation";
            DataTable table = repository.ExecuteQuery(query);
            List<PresentationDTO> presentations = new List<PresentationDTO>();
            foreach (DataRow row in table.Rows)
            {
                presentations.Add(RowToPresentation(row));
            }
            return presentations;
        }

        public bool UpdatePresentation(PresentationDTO presentation)
        {
            string titleEscaped = presentation.Title.Replace("'", "''");
            string descriptionEscaped = presentation.Description.Replace("'", "''");

            // Format dates and enums properly.
            string formattedDate = presentation.Date.ToString("yyyy-MM-dd");
            string formattedTime = presentation.Hour.ToString();
            string sectionAsString = presentation.Section.ToString();

            string query = $@"
        UPDATE presentation
        SET
            title = '{titleEscaped}',
            description = '{descriptionEscaped}',
            date = '{formattedDate}',
            hour = '{formattedTime}',
            section = '{sectionAsString}',
            id_conference = {presentation.IdConference},
            id_author = {presentation.IdAuthor}
        WHERE id = {presentation.Id}";
            return repository.ExecuteNonQuery(query);
        }

        public bool DeletePresentation(int id)
        {
            string query = $"DELETE FROM presentation WHERE id = {id}";
            return repository.ExecuteNonQuery(query);
        }

        public bool UpsertPresentation(PresentationDTO presentation)
        {
            if (ReadPresentationById(presentation.Id) == null)
            {
                return CreatePresentation(presentation);
            }
            return UpdatePresentation(presentation);
        }

        //Filter methods
        public List<PresentationDTO> ReadPresentationsBySection(Section section)
        {
            string query = $"SELECT * FROM presentation WHERE section = '{section}'";
            DataTable table = repository.ExecuteQuery(query);
            List<PresentationDTO> presentations = new List<PresentationDTO>();
            foreach (DataRow row in table.Rows)
            {
                presentations.Add(RowToPresentation(row));
            }
            return presentations;
        }


    }
}
