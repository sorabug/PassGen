using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("欢迎使用密码生成器!");

            // Prompt the user for the password length
            Console.Write("请输入密码生成长度: ");
            string passwordLengthInput = Console.ReadLine();

            // Validate the password length input
            int passwordLength;
            while (!int.TryParse(passwordLengthInput, out passwordLength) || passwordLength <= 0)
            {
                Console.WriteLine("无效的密码长度。请输入一个正整数");
                Console.Write("输入密码生成长度: ");
                passwordLengthInput = Console.ReadLine();
            }

            // Generate the password
            string password = GeneratePassword(passwordLength);

            // Print the password and save it to a file
            Console.WriteLine("您的密码是: " + password);
            File.WriteAllText("pw.txt", password);
            Console.WriteLine("密码已被保存到pw.txt中.");
            Console.ReadLine();
        }

        static string GeneratePassword(int length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;':,.<>?";
            StringBuilder password = new StringBuilder(length);
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    password.Append(validCharacters[(int)(num % (uint)validCharacters.Length)]);
                }
            }

            return password.ToString();
        }
    }
}
