namespace Server.Domain.Model
{
    public class Participant
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CNP { get; set; }
        public string PdfFilePath { get; set; }
        public string PhotoFilePath { get; set; }

        public Participant(int id, string name, string email, string phone, string cnp, string pdfFilePath, string photoFilePath)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            CNP = cnp;
            PdfFilePath = pdfFilePath;
            PhotoFilePath = photoFilePath;
        }
        
        public Participant()
        {
            Id = 0;
            Name = "";
            Email = "";
            Phone = "";
            CNP = "";
            PdfFilePath = "";
            PhotoFilePath = "";
        }

        public Participant(Participant participant)
        {
            Id = participant.Id;
            Name = participant.Name;
            Email = participant.Email;
            Phone = participant.Phone;
            CNP = participant.CNP;
            PdfFilePath = participant.PdfFilePath;
            PhotoFilePath = participant.PhotoFilePath;
        }
        
        public static List<Participant> dummyParticipantData()
        {
            List<Participant> participants = new List<Participant>();
            participants.Add(new Participant(1, "Alex Green", "alex.green@example.com", "555-1234", "1234567890123", "alex-green-cv.pdf", "alex-green-photo.jpg"));
            participants.Add(new Participant(2, "Samantha Blue", "s.blue@example.com", "555-5678", "1234567890124", "samantha-blue-portfolio.pdf", "samantha-blue-photo.jpg"));
            participants.Add(new Participant(3, "Chris Yellow", "chris.yellow@example.com", "555-9012", "1234567890125", "chris-yellow-research.pdf", "chris-yellow-photo.jpg"));
            participants.Add(new Participant(4, "Patricia White", "patricia.white@example.com", "555-3456", "1234567890126", "patricia-white-paper.pdf", "patricia-white-photo.jpg"));
            participants.Add(new Participant(5, "Daniel Brown", "d.brown@example.com", "555-7890", "1234567890127", "daniel-brown-resume.pdf", "daniel-brown-photo.jpg"));
            participants.Add(new Participant(6, "Anca Iordan", "anca.iordan@cs.utcluj.ro", "555-5678", "9876543210987", "anca-iordan-cv.pdf", "anca-iordan-photo.jpg"));
            return participants;
        }
        public static List<(int,int)> dummyRelations()
        {            
            List<(int, int)> relations = new List<(int, int)>();
            relations.Add((1, 1));
            relations.Add((2, 2));
            relations.Add((3, 3));
            relations.Add((1, 2));
            relations.Add((2, 1));
            relations.Add((3, 4));
            return relations;
        }
    }
}
