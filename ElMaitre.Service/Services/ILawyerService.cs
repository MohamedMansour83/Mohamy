using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface ILawyerService
    {
        IEnumerable<LawyerDTO> GetLawyers();
        LawyerDTO GetLawyer(long id);
        LawyerDTO GetLawyerByUserId(string id);
        LawyerDTO GetLawyerByFbId(string id);
        IEnumerable<LawyerDTO> GetLawyers(int ServiceId,string Name, int Specialization,Gender? Gender, List<int> Rating,float minFees,float maxFees, List<int> Prices,bool? isOnline, List<int> Experiences);
        int InsertLawyer(LawyerDTO lawyer);
        void UpdateLawyer(LawyerDTO lawyer);
        IEnumerable<LawyerSpecializationDTO> GetSpetializations();
        RatingDTO GetRating(long id);

        IEnumerable<KeyValueDTO> GetLawyerExperiences();

    }
}
