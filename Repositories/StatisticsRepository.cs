using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Repositories
{
    internal class StatisticsRepository
    {
        private Repository repository;
        private ParticipantRepository participantRepository;
        private PresentationRepository presentationRepository;
        private UserRepository userRepository;

        public StatisticsRepository()
        {
            repository = Repository.Instance;
            participantRepository = new ParticipantRepository();
            presentationRepository = new PresentationRepository();
            userRepository = new UserRepository();
        }

        // Get the number of participants by conference
        public Dictionary<string, int> GetNumberOfParticipantsByConference()
        {
            string query = @"
                SELECT c.title, COUNT(pp.id_participant) as num_participants
                FROM conference c
                JOIN presentation p ON c.id = p.id_conference
                JOIN presentation_participant pp ON p.id = pp.id_presentation
                GROUP BY c.title";

            DataTable dataTable = repository.ExecuteQuery(query);
            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (DataRow row in dataTable.Rows)
            {
                string conferenceTitle = row["title"].ToString();
                int numParticipants = Convert.ToInt32(row["num_participants"]);
                result[conferenceTitle] = numParticipants;
            }

            return result;
        }

        // Get the number of participants by section
        public Dictionary<string, int> GetNumberOfParticipantsBySection()
        {
            string query = @"
                SELECT p.section, COUNT(pp.id_participant) as num_participants
                FROM presentation p
                JOIN presentation_participant pp ON p.id = pp.id_presentation
                GROUP BY p.section";

            DataTable dataTable = repository.ExecuteQuery(query);
            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (DataRow row in dataTable.Rows)
            {
                string section = row["section"].ToString();
                int numParticipants = Convert.ToInt32(row["num_participants"]);
                result[section] = numParticipants;
            }

            return result;
        }

        // Get the number of presentations by author
        public Dictionary<string, int> GetNumberOfPresentationsByAuthor()
        {
            string query = @"
                SELECT ua.name as author_name, COUNT(p.id) as num_presentations
                FROM useraccount ua
                JOIN presentation p ON ua.id = p.id_author
                GROUP BY ua.name";

            DataTable dataTable = repository.ExecuteQuery(query);
            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (DataRow row in dataTable.Rows)
            {
                string authorName = row["author_name"].ToString();
                int numPresentations = Convert.ToInt32(row["num_presentations"]);
                result[authorName] = numPresentations;
            }

            return result;
        }

        // Get the number of presentations per day
        public Dictionary<DateTime, int> GetNumberOfPresentationsPerDay()
        {
            string query = @"
                SELECT p.date, COUNT(p.id) as num_presentations
                FROM presentation p
                GROUP BY p.date
                ORDER BY p.date";

            DataTable dataTable = repository.ExecuteQuery(query);
            Dictionary<DateTime, int> result = new Dictionary<DateTime, int>();

            foreach (DataRow row in dataTable.Rows)
            {
                DateTime date = Convert.ToDateTime(row["date"]);
                int numPresentations = Convert.ToInt32(row["num_presentations"]);
                result[date] = numPresentations;
            }

            return result;
        }
    }
}
