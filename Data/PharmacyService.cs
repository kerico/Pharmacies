using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Pharmacies.Data
{
    public interface IPharmacyService
    {
        public List<Pharmacy> GetPharmacies();
        public void ImportPharmacy(Pharmacy pharmacy);
        public void ImportPharmacies(IEnumerable<Pharmacy> pharmacies);
        public void UpdatePostCodes();

    }
    public class PharmacyService : IPharmacyService
    {
        IDbContextFactory<PharmacyContext> _dbFactory;
        PharmaciesLogger _logger;
        IConfiguration _configuration;
        public PharmacyService(IDbContextFactory<PharmacyContext> dbFactory, PharmaciesLogger logger, IConfiguration configuration)
        {
            _dbFactory = dbFactory;
            _logger = logger;   
            _configuration = configuration;
        }
        public List<Pharmacy> GetPharmacies()
        {
            using var context = _dbFactory.CreateDbContext();
            return context.Pharmacies?.ToList() ?? new List<Pharmacy>();
        }

        public void ImportPharmacies(IEnumerable<Pharmacy> pharmacies) //TODO: return how many inserted/updated
        {
            try
            {
                var names = pharmacies.Select(x => x.Name);
                using var context = _dbFactory.CreateDbContext();
                var toUpdate = context.Pharmacies.UpdateValues(pharmacies);
                context.Pharmacies.UpdateRange(toUpdate);
                context.SaveChanges();
                _logger.WriteLog(nameof(ImportPharmacies), toUpdate);
            }
            catch (Exception ex)
            {
                //TODO: handle exceptions to retduce code duplication
                if (ex.InnerException != null && ex.InnerException is SqliteException sqliteException)
                {
                    switch (sqliteException.SqliteExtendedErrorCode)
                    {
                        case 2067: throw new ValidationException("Parameter 'Name' must be unique"); 
                        default:
                            throw;
                    }
                }
                throw;
            }
        }

        public void ImportPharmacy(Pharmacy pharmacy)
        {
            try
            {
                using var context = _dbFactory.CreateDbContext();
                context.Pharmacies.Update(pharmacy);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is SqliteException sqliteException)
                {
                    switch (sqliteException.SqliteExtendedErrorCode)
                    {
                        case 2067: throw new ValidationException("Parameter 'Name' must be unique"); 
                        default:
                            throw;
                    }
                }
                throw;
            }
        }

        public void UpdatePostCodes()
        {
            using var context = _dbFactory.CreateDbContext();
            var pharmacies = context.Pharmacies;
            if (pharmacies == null || !pharmacies.Any())
                return;
            var client = new PostiitClient(_configuration);
            foreach (var pharmacy in pharmacies)
            {
                var postCode = client.GetPostCode(pharmacy.Address);
                pharmacy.PostCode = postCode;
            }
            context.SaveChanges();
            _logger.WriteLog(nameof(UpdatePostCodes), pharmacies.ToArray());
        }
    }
}
