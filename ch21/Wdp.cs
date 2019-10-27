using System;
using System.Linq;
using System.Security.Principal;
using System.Security.Cryptography;
namespace ch21
{
    public class Wdp
    {
        public static void Run()
        {
            Console.WriteLine($"Running as {WindowsIdentity.GetCurrent().Name}");

            byte[] original = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Original: {string.Join(", ", original)}");

            byte[] encryptedUser = ProtectedData.Protect(original, null, DataProtectionScope.CurrentUser);
            Console.WriteLine($"Encrypted (user): {string.Join(", ", encryptedUser)}");

            byte[] encryptedMachine = ProtectedData.Protect(original, null, DataProtectionScope.LocalMachine);
            Console.WriteLine($"Encrypted (machine): {string.Join(", ", encryptedMachine)}");

            byte[] decryptedUser = ProtectedData.Unprotect(encryptedUser, null, DataProtectionScope.CurrentUser);
            Console.WriteLine($"Decrypted (user): {string.Join(", ", decryptedUser)}");

            byte[] decryptedMachine = ProtectedData.Unprotect(encryptedMachine, null, DataProtectionScope.LocalMachine);
            Console.WriteLine($"Decrypted (machine): {string.Join(", ", decryptedMachine)}");

        }
    }
}