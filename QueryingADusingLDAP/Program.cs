using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;

namespace QueryingADusingLDAP
{
    class Program
    {
        static void Main(string[] args)
        {
            //string domainPath = "LDAP://kpsk.care";
            //string domainPath = "LDAP://DC=kpsk, DC=care";  //Local Windows Server
            string domainPath = "LDAP://ldaps.kpsk.care:636";
            //string domainPath = "LDAP://51.105.11.208:636";

            DirectoryEntry searchRoot = new DirectoryEntry(domainPath, "appdeploy@kpsk.care", "Nuhu2968Nuhu2968", System.DirectoryServices.AuthenticationTypes.Secure);

            DirectorySearcher search = new DirectorySearcher(searchRoot);
            //search.Filter = "(&(objectClass=user)(objectCategory=person))";
            search.Filter = "(&(&(objectClass=user)(objectCategory=person))(|(memberOf=CN=CupAdmin,OU=AADDC Users,DC=kpsk,DC=care)(memberOf=CN=CupManagers,OU=AADDC Users,DC=kpsk,DC=care)(memberOf=CN=CupUsers,OU=AADDC Users,DC=kpsk,DC=care)))";
            search.Sort = new SortOption("name", SortDirection.Ascending);

            search.PropertiesToLoad.Add("objectguid");
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("name");
            search.PropertiesToLoad.Add("userPrincipalName");   //User login name
            search.PropertiesToLoad.Add("cn");                  // Login Name
            search.PropertiesToLoad.Add("givenName");           // First Name
            search.PropertiesToLoad.Add("sn");                  // Last Name
            search.PropertiesToLoad.Add("displayName");         // Display Name
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("TelephoneNumber");
            search.PropertiesToLoad.Add("memberof");            //Get User Groups
            search.PropertiesToLoad.Add("usergroup");

            SearchResultCollection resultCol = search.FindAll();

            SearchResult result;
            var users = new List<ADUserByGroup>();

            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    result = resultCol[counter];
                    var user = new ADUserByGroup();

                    if (result.Properties.Contains("objectGUID"))
                    {
                        user.UserId = new Guid(result.Properties["objectGUID"][0] as byte[]).ToString();
                    }
                    if (result.Properties.Contains("samaccountname"))
                    {
                        user.AccountName = result.Properties["samaccountname"][0].ToString();
                    }
                    if (result.Properties.Contains("name"))
                    {
                        user.Name = result.Properties["name"][0].ToString();
                    }
                    if (result.Properties.Contains("userPrincipalName"))
                    {
                        user.UserPrincipalName = result.Properties["userPrincipalName"][0].ToString();
                    }
                    if (result.Properties.Contains("cn"))
                    {
                        user.UserName = result.Properties["cn"][0].ToString();
                    }
                    if (result.Properties.Contains("givenName"))
                    {
                        user.FullName = result.Properties["givenName"][0].ToString() + " ";
                    }
                    if (result.Properties.Contains("sn"))
                    {
                        user.FullName = user.FullName + "" + result.Properties["sn"][0].ToString();
                    }
                    if (result.Properties.Contains("displayName"))
                    {
                        user.DisplayName = result.Properties["displayName"][0].ToString();
                    }
                    if (result.Properties.Contains("mail"))
                    {
                        user.Email = result.Properties["mail"][0].ToString();
                    }
                    if (result.Properties.Contains("TelephoneNumber"))
                    {
                        user.ContactNo = result.Properties["TelephoneNumber"][0].ToString();
                    }
                    if (result.Properties.Contains("memberof"))
                    {
                        user.MemberOf = result.Properties["memberof"][0].ToString();
                    }
                    if (result.Properties.Contains("usergroup"))
                    {
                        user.Group = result.Properties["usergroup"][0].ToString();
                    }
                    users.Add(user);
                }
            }

            string text = null;
            foreach (var item in users)
            {
                text = text + "\n" + "UserId-" + item.UserId + ", UserName-" + item.UserName + ", Email-" + item.Email + ", ContactNo-" + item.ContactNo + ", DisplayName-" + item.DisplayName + ", FullName-" + item.FullName + ", AccountName-" + item.AccountName + ", Group-" + item.Group;
                Console.WriteLine("UserId-" + item.UserId + ", UserName-" + item.UserName + ", Email-" + item.Email + ", ContactNo-" + item.ContactNo + ", DisplayName-" + item.DisplayName + ", FullName-" + item.FullName + ", AccountName-" + item.AccountName + ", Group-" + item.Group);
            }
            File.WriteAllText(@"C:\\UserInformation.txt", text);

            Console.ReadKey();
        }
    }
}
