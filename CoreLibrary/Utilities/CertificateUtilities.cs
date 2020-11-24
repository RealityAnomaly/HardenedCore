using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Security;
using Net.Pkcs11Interop.X509Store;

namespace CoreLibrary.Utilities
{
    public class CertificateUtilities
    {
        private class PinProvider : IPinProvider
        {
            public GetPinResult GetKeyPin(Pkcs11X509StoreInfo storeInfo, Pkcs11SlotInfo slotInfo, Pkcs11TokenInfo tokenInfo, Pkcs11X509CertificateInfo certificateInfo)
            {
                throw new NotImplementedException();
            }

            public GetPinResult GetTokenPin(Pkcs11X509StoreInfo storeInfo, Pkcs11SlotInfo slotInfo, Pkcs11TokenInfo tokenInfo)
            {
                Console.Write("PIN: ");
                var pin = ReadPassword();

                return new GetPinResult(false, Encoding.UTF8.GetBytes(pin));
            }

            internal static string ReadPassword()
            {
                var password = new StringBuilder();
                while (true)
                {
                    var info = Console.ReadKey(true);
                    if (info.Key == ConsoleKey.Enter)
                    {
                        Console.Write('\n');
                        break;
                    }
                    else if (info.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length <= 0) continue;

                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                    else if (info.KeyChar != '\u0000')
                    {
                        password.Append(info.KeyChar);
                        Console.Write('*');
                    }
                }

                return password.ToString();
            }
        }

        public static byte[] GenerateEncryptionKey(int keySize)
        {
            // This is of course OAEP padding. Fuck PKCS#1 and its 11 bytes
            return EncryptionUtilities.GenerateRandomBytes((keySize / 8) - 42);
        }

        public static X509Certificate2 GetCertificateFromSerial(string serial)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false); // TODO: Temporary Override for expired
            if (certs.Count > 0) return certs[0];

            return null;
        }

        public static Pkcs11X509Certificate GetPkcs11CertificateFromSerial(string serial) {
            var store = new Pkcs11X509Store("/nix/store/vyy3qaa99ldb2f2f3kgva7z97c3rjwcw-opensc-0.20.0/lib/opensc-pkcs11.so", new PinProvider());
            foreach (var slot in store.Slots) {
                // no card is in the reader
                if (slot.Token == null) continue;

                var pcerts = slot.Token.Certificates.FindAll(c => c.Info.ParsedCertificate.SerialNumber == serial);
                if (pcerts.Count > 0) return pcerts[0];
            }

            return null;
        }
    }
}
