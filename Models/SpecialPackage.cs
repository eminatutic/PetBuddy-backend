namespace api.Models
{
    public class SpecialPackage
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string PackageType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<SpecialPackagePet> SpecialPackagePets { get; set; } = new List<SpecialPackagePet>();

    }
}