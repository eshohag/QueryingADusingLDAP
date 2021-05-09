namespace QueryingADusingLDAP
{
    public class ADUserByGroup
    {
        public string UserId { get; set; }
        public string UserPrincipalName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string AccountName { get; set; }
        public string MemberOf { get; set; }
        public string Group { get; set; }
        public string Name { get; internal set; }
    }
}
