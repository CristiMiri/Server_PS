using Server.Domain.DTO;
using Server.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    internal interface IPresentationService
    {
        bool CreatePresentation(PresentationDTO presentationDTO);
        bool UpdatePresentation(PresentationDTO presentationDTO);
        bool DeletePresentation(int id);

        PresentationDTO GetPresentation(int id);
        List<PresentationDTO> GetAllPresentation();
        List<PresentationDTO> GetPresentationsbySection(Section section);
    }
}
