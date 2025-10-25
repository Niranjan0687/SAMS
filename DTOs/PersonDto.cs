namespace SmsAPI.DTOs
{
    public class PersonDto
    {
        public int BusinessEntityID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string PersonType { get; set; }

        public bool NameStyle { get; set; }
    }
}
