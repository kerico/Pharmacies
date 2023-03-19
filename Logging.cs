using Microsoft.EntityFrameworkCore;
using Pharmacies.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Pharmacies
{
    public class PharmaciesLogger
    {
        IDbContextFactory<PharmacyContext> _dbFactory;
        public PharmaciesLogger(IDbContextFactory<PharmacyContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public void WriteLog(string action, params Pharmacy[] pharmacies)
        {
            if (pharmacies.Length < 1)
                return;

            using var context = _dbFactory.CreateDbContext();
            foreach (var pharmacy in pharmacies)
            {

                var newLog = new OperationLog
                {
                    Date = DateTime.Now,
                    Operation = action,
                    PharmacyID = pharmacy.ID,
                };
                context.Add(newLog);
            }
            context.SaveChanges();
        }
    }
}
