using Server.Domain.DTO;
using Server.Domain.Model;
using Server.Repositories;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class PresentationService: IPresentationService
    {
        private PresentationRepository _presentationRepository;
        public PresentationService()
        {
            _presentationRepository = new PresentationRepository();
        }

        public bool CreatePresentation(PresentationDTO presentationDTO)
        {
            return _presentationRepository.CreatePresentation(presentationDTO);
        }

        public bool DeletePresentation(int id)
        {
            return _presentationRepository.DeletePresentation(id);
        }

        public List<PresentationDTO> GetAllPresentation()
        {
            return _presentationRepository.ReadPresentations();
        }

        public PresentationDTO GetPresentation(int id)
        {
            return _presentationRepository.ReadPresentationById(id);
        }

        public List<PresentationDTO> GetPresentationsbySection(Section section)
        {
            return _presentationRepository.ReadPresentationsBySection(section);
        }

        public bool UpdatePresentation(PresentationDTO presentationDTO)
        {
            return (_presentationRepository.UpdatePresentation(presentationDTO));
        }
    }
}
