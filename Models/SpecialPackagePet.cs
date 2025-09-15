namespace api.Models
{
    public class SpecialPackagePet
    {
        public int SpecialPackageId { get; set; }
        public SpecialPackage SpecialPackage { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}