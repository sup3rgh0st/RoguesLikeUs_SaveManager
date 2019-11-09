using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLU_Save_Encrypter
{
    class Program
    {
        private static string _vector = "8947az34awl34kjq";
        private static string _salt = "aselrias38490a32";
        private static int _keySize = 256;
        private static string _hash = "SHA1";
        private static int _iterations = 2;
        private static string password = "testpass";

        static void Main(string[] args) {
            if (args.Length != 1) {
                Console.WriteLine("Click and Drag Decrypted JSON save onto program.");
                Console.WriteLine("Press any Key to Continue...");
                Console.ReadKey();
                return;
            }
            String input = "";
            using (StreamReader sr = new StreamReader(args[0])) {
                input = sr.ReadToEnd();
                Console.WriteLine(input);
            }

            byte[] bytes = Encoding.ASCII.GetBytes(_vector);
            byte[] bytes2 = Encoding.ASCII.GetBytes(_salt);
            byte[] bytes3 = Encoding.UTF8.GetBytes(input);
            AesManaged val = new AesManaged();
            byte[] inArray;
            try {
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password, bytes2, _hash, _iterations);
                byte[] bytes4 = passwordDeriveBytes.GetBytes(_keySize / 8);
                val.Mode = CipherMode.CBC;
                using (ICryptoTransform transform = val.CreateEncryptor(bytes4, bytes)) {
                    using (MemoryStream memoryStream = new MemoryStream()) {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write)) {
                            cryptoStream.Write(bytes3, 0, bytes3.Length);
                            cryptoStream.FlushFinalBlock();
                            inArray = memoryStream.ToArray();
                        }
                    }
                }
                val.Clear();
            } finally {
                ((IDisposable)val)?.Dispose();
            }

            System.IO.File.WriteAllText(@".\encrypted_save.txt", Convert.ToBase64String(inArray));
            Console.WriteLine("Press any Key to Continue...");
            Console.ReadKey();
            //return Convert.ToBase64String(inArray);
        }
    }
}
