using System;
using System.Collections.Generic;

#nullable disable

namespace petsLighthouseAPI
{
    public partial class Sector
    {
        public Sector()
        {
            PostPets = new HashSet<PostPet>();
        }

        public int IdSector { get; set; }
        public string Name { get; set; }
        public int IdCanton { get; set; }

        public virtual Canton IdCantonNavigation { get; set; }
        public virtual ICollection<PostPet> PostPets { get; set; }
    }
}
