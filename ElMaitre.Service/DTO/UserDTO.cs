using ElMaitre.DAL.Models;
using System.Collections;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public IEnumerable<SesstionDTO> Sessions { get; set; }

    }
}
