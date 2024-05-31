namespace Server.Domain.Model
{
    public class Conference
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }

        public Conference(int id, string title, string location, DateTime date)
        {
            this.Id = id;
            this.Title = title;
            this.Location = location;
            this.Date = date;
        }

        public Conference(Conference conference)
        {
            this.Id = conference.Id;
            this.Title = conference.Title;
            this.Location = conference.Location;
            this.Date = conference.Date;
        }

        public Conference()
        {
            this.Id = -1;
            this.Title = "";
            this.Location = "";
            this.Date = new DateTime();
        }

        public List<Conference> dummyConferenceData()
        {
            List<Conference> conferences = new List<Conference>();
            conferences.Add(new Conference(1, "AI Conference", "New York", new DateTime(2024, 4, 10)));
            conferences.Add(new Conference(2, "Web Development Summit", "San Francisco", new DateTime(2024, 4, 15)));
            conferences.Add(new Conference(3, "Medical Research Symposium", "Chicago", new DateTime(2024, 5, 5)));
            return conferences;
        }
    }
}
