using Pharmacies.Data;

namespace Pharmacies
{
    public static class Extensions
    {
        public static Pharmacy[] UpdateValues(this IEnumerable<Pharmacy> original, IEnumerable<Pharmacy> pharmacies)
        {
            foreach (var pharmacy in pharmacies)
            {
                var originalEntry = original.FirstOrDefault(x => x.Name == pharmacy.Name);
                if (originalEntry != null)
                {
                    pharmacy.ID = originalEntry.ID;
                    originalEntry.Name = pharmacy.Name;
                    originalEntry.Address = pharmacy.Address;
                    originalEntry.PostCode = pharmacy.PostCode;
                }
                original = original.Union(pharmacies.Where(x => x.ID == 0));
            }
            return original.ToArray();
        }
    }
}
