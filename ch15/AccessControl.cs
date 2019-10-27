using System;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ch15
{
    public class AccessControl
    {
        static public void Run()
        {
            var file = "sectest.txt";
            File.WriteAllText(file, "File security.");

            var sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            string usersAccount = sid.Translate(typeof(NTAccount)).ToString();

            Console.WriteLine($"User: {usersAccount}");


            FileSecurity sec = new FileSecurity(file,
                                      AccessControlSections.Owner |
                                      AccessControlSections.Group |
                                      AccessControlSections.Access);

            Console.WriteLine("AFTER CREATE:");
            ShowSecurity(sec);

            sec.ModifyAccessRule(AccessControlModification.Add,
                new FileSystemAccessRule(usersAccount, FileSystemRights.Write, AccessControlType.Allow),
                out bool modified);

            Console.WriteLine("AFTER MODIFY:");

            ShowSecurity(sec);

            File.Delete(file);
        }
        static void ShowSecurity(FileSecurity sec)
        {
            AuthorizationRuleCollection rules = sec.GetAccessRules(true, true,
                                                                 typeof(NTAccount));
            foreach (FileSystemAccessRule rule in rules.Cast<FileSystemAccessRule>()
              .OrderBy(r => r.IdentityReference.Value))
            {
                // e.g., MyDomain/Joe
                Console.WriteLine($"  {rule.IdentityReference.Value}");
                // Allow or Deny: e.g., FullControl
                Console.WriteLine($"    {rule.FileSystemRights}: {rule.AccessControlType}");
            }
        }

    }
}