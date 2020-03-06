using System;
using System.Collections.Generic;
using System.Linq;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Store;
using ConsoleTables;

namespace DRXUtility
{
    public static class DebugHelper
    {
        public static void DumpDocument(DrxDocument document) {
            Console.WriteLine("DRX Document Version 3.02");
            Console.WriteLine();
            Console.WriteLine("<=== Section: Header ===>");
            Console.WriteLine($"Document ID: {document.Id}");
            Console.WriteLine($"Store ID: {document.Header.Store}");
            Console.WriteLine($"Title: {document.Header.Title}");
            Console.WriteLine($"VREL: {document.Header.Vrel}");
            Console.WriteLine($"Time Stamp: {document.Header.TimeStamp}");
            Console.WriteLine($"Security Level: {document.Header.SecurityLevel}");
            Console.WriteLine($"Encryption Enabled: {document.Header.Encrypted}");

            var key = document.Header.Key;
            if (key != null) {
                Console.WriteLine($"  Key ID: {key.KeyId}");
                Console.WriteLine($"  Key Protectors:");
                foreach (var protector in key.Protectors) {
                    Console.WriteLine($"    - {protector.Protector.ProtectorName} with Intent: {protector.Intent}");
                    if (protector.Protector.ProtectorKey != null)
                        Console.WriteLine($"      Key Present, {(protector.Protector.ProtectorKey.Length - 36) * 8} bits");
                    if (protector.Protector.ProtectorState != null)
                        Console.WriteLine($"      State Present, {protector.Protector.ProtectorState.Length} elements");
                    if (protector.Protector.Unlocked) {
                        Console.WriteLine("      Key is Unlocked");
                    } else {
                        Console.WriteLine("      Key is Locked");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("<=== Section: Body ===>");
            Console.WriteLine($"Body Type: {document.Header.BodyType}");
            Console.WriteLine($"Body Length: {document.Body.Length} bytes");
        }
    }
}