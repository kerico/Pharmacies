namespace Pharmacies.Data
{
    public class OperationLog
    {
        public int ID { get; set; }
        public string Operation { get; set; }
        public DateTime? Date { get; set; }
        public int PharmacyID { get; set; }
    }
}
