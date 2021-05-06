using System.DirectoryServices;

namespace QueryingADusingLDAP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a directory entry object by passing LDAP address

            string domainPath = "LDAP://kpsk.care";
            //string domainPath = "LDAP://DC=kpsk, DC=care";  //Local Windows Server
            //string domainPath = "LDAP://ldaps.kpsk.care";   //
            //string domainPath = "LDAP://51.105.11.208:636";

            DirectoryEntry searchRoot = new DirectoryEntry(domainPath, "appdeploy@kpsk.care", "Jofu9369Jofu9369", AuthenticationTypes.Secure);
            //DirectoryEntry searchRoot = new DirectoryEntry(domainPath, null, null, AuthenticationTypes.None);

            DirectorySearcher search = new DirectorySearcher(searchRoot);
            search.Filter = "(&(objectClass=user)(objectCategory=person))";
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("usergroup");
            search.PropertiesToLoad.Add("displayname");//first name

            SearchResultCollection resultCol = search.FindAll();
        }
    }
}