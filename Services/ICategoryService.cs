using petsLighthouseAPI.Models;
using System.Collections.Generic;

namespace petsLighthouseAPI.Services
{
    public interface ICategoryService
    {
        List<PetSpecieView> GetPetSpecie();
        List<PetBreedView> GetPetBreeds();
        List<PetBreedView> GetPetBreedsBySpecie(int id);
        List<ProvinciaView> GetProvincias();
        List<CantonView> GetCantons();
        List<CantonView> GetCantonsByProv(int id);
        List<SectorView> GetSectors();
        List<SectorView> GetSectorsByCanton(int id);
    }
}
