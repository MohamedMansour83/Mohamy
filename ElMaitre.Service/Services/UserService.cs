using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElMaitre.Services
{
    public class UserService : IUserService
    {
        private IRepository<Lawyer> LawyerRepository;
        private IRepository<LawyerSpecialization> LawyerSpecializationRepository;

        public UserService(IRepository<Lawyer> LawyerRepository, IRepository<LawyerSpecialization> LawyerSpecializationRepository)
        {
            this.LawyerRepository = LawyerRepository;
            this.LawyerSpecializationRepository = LawyerSpecializationRepository;
        }


        public UserDTO GetUserDetails(long id)
        {
            var lawyer = LawyerRepository.Get(s => s.Id == id, includeProperties: "Specialization").FirstOrDefault();
            if (lawyer == null)
                return null;

            return new UserDTO
            {
               
            };
        }
    }
}
