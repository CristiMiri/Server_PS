
using Server.Domain.DTO;
using Server.Domain.Model;
using Server.Repositories;
using Server.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Server.Services
{
    internal class ParticipantService : IParticipantService
    {
        private ParticipantRepository _participantRepository;
        private Participant_PrezentareRepository _participant_PrezentareRepository;        
        private EmailSender _emailSender;
        private FileManager _fileManager;
        public ParticipantService()
        {
            _participantRepository = new ParticipantRepository();
            _emailSender = new EmailSender();
            _fileManager = new FileManager();
            _participant_PrezentareRepository = new Participant_PrezentareRepository(new ParticipantRepository(), new PresentationRepository());
        }

        public bool CreateParticipant(ParticipantDTO participantDTO)
        {
            return _participantRepository.CreateParticipant(participantDTO);
        }
        public bool UpdateParticipant(ParticipantDTO participantDTO)
        {
            return _participantRepository.UpdateParticipant(participantDTO);
        }
        public bool DeleteParticipant(int id)
        {
            return _participantRepository.DeleteParticipant(id);
        }

        public ParticipantDTO GetParticipant(int id)
        {
            return _participantRepository.ReadParticipantById(id);
        }
        public List<ParticipantDTO> GetAll()
        {
            return _participantRepository.ReadParticipants();
        }
        public List<ParticipantDTO> GetParticipantsbySection(Section section)
        {
            return _participantRepository.GetParticipantsBySection(section);
        }
        public int GetLastParticipantId()
        {
            return _participantRepository.GetLastParticipantId();
        }


        //Email methods
        public void AcceptParticipant(string email)
        {
            string subject = "Participare acceptata";
            string body = "Felicitari! Participarea dumneavoastra a fost acceptata! Va asteptam la eveniment!";
            _emailSender.SendMail(email, subject, body);
            Console.WriteLine("Email sent to " + email);

        }
        public void RejectParticipant(string email)
        {
            string subject = "Participare respinsa";
            string body = "Ne pare rau, dar participarea dumneavoastra a fost respinsa.";
            _emailSender.SendMail(email, subject, body);
            Console.WriteLine("Email sent to " + email);
        }

        public Dictionary<string, byte[]> GetParticipantsPhotos()
        {
            List<ParticipantDTO> participants = _participantRepository.ReadParticipants();
            //ParticipantDTO participant1 = _participantRepository.ReadParticipantById(1);
            var photos = new Dictionary<string, byte[]>();
            foreach (var participant in participants)
            {
                string filePath = _fileManager.GetImageFilePath(participant.PhotoFilePath);
                if (File.Exists(filePath))
                {
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    //photos.Add(participant.PhotoFilePath, fileBytes);
                    photos[participant.PhotoFilePath] = fileBytes;
                }
                else
                {

                    photos.Add(participant.PhotoFilePath, null);
                }
            }
            //string filePath = _fileManager.GetImageFilePath(participant1.PhotoFilePath);
            //byte[] fileBytes = File.ReadAllBytes(filePath);
            //photos.Add(participant1.PhotoFilePath, fileBytes);
            return photos;
        }

        public Dictionary<string, byte[]> GetParticipantCV(string Path)
        {
            //ParticipantDTO participant = _participantRepository.ReadParticipantById(id);
            string filePath = _fileManager.GetDocumentFilePath(Path);
            var file = new Dictionary<string, byte[]>();
            if (File.Exists(filePath))
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                file.Add(Path, fileBytes);
                return file;
            }
            else
            {
                return null;
            }
        }

        internal bool SaveParticipantPhoto(Dictionary<string, byte[]>? photo)
        {
            if (photo == null)
            {
                return false;
            }
            foreach (var item in photo)
            {
                //Convert the byte array to image                
                string filePath = _fileManager.GetImageFilePath(item.Key);                
                File.WriteAllBytes(filePath, item.Value);

            }
            return true;
        }

        internal bool SaveParticipantCV(Dictionary<string, byte[]>? cvData)
        {
            if (cvData == null)
            {
                return false;
            }
            foreach (var item in cvData)
            {
                string filePath = _fileManager.GetDocumentFilePath(item.Key);
                File.WriteAllBytes(filePath, item.Value);
            }
            return true;
        }

        internal bool CreateParticipantPresentation(int idParticipant,int idPresentation)
        {
            Console.WriteLine("CreateParticipantPresentation" + idParticipant + " " + idPresentation);
            bool result = _participant_PrezentareRepository.CreateParticipantPresentation(idParticipant, idPresentation);
            return result;
        }

        internal bool UpsertParticipant(ParticipantDTO? participantDTO)
        {
            return _participantRepository.UpsertParticipant(participantDTO);
        }
    }
}
