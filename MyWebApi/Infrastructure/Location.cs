namespace MembershipEligibilitySearch.Api.Infrastructure
{
    internal class Location
    {
        public int Limit { get; set; }
        public int Total { get; set; }
        public int Offset { get; set; }
        public string Next { get; internal set; }
        public string Previous { get; internal set; }
    }
}