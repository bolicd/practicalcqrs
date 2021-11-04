namespace Infrastructure.Model.ReadModels
{
    public class PersonReadModel : ReadModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string AggregateId { get; set; }
    }
}
