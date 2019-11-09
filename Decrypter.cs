using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLU_Save_Decrypter
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
                Console.WriteLine("Click and Drag Encrypted save onto program.");
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
            byte[] array = Convert.FromBase64String(input);
            int count = 0;
            AesManaged val = new AesManaged();
            byte[] array2;
            try {
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password, bytes2, _hash, _iterations);
                byte[] bytes3 = passwordDeriveBytes.GetBytes(_keySize / 8);
                val.Mode = CipherMode.CBC;
                try {
                    using (ICryptoTransform transform = val.CreateDecryptor(bytes3, bytes)) {
                        using (MemoryStream stream = new MemoryStream(array)) {
                            using (CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read)) {
                                array2 = new byte[array.Length];
                                count = cryptoStream.Read(array2, 0, array2.Length);
                            }
                        }
                    }
                } catch (Exception) {
                    return;// string.Empty;
                }
                val.Clear();
            } finally {
                ((IDisposable)val)?.Dispose();
            }
            //return Encoding.UTF8.GetString(array2, 0, count);

            System.IO.File.WriteAllText(@".\decrypted_save.txt", Encoding.UTF8.GetString(array2, 0, count));
            Console.WriteLine("Press any Key to Continue...");
            Console.ReadKey();
            //return Convert.ToBase64String(inArray);
        }
    }
}
