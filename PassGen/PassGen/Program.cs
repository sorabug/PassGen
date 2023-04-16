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
            Console.Write("请输入所需的密码长度(请使用正整数): ");
            string passwordLengthInput = Console.ReadLine();

            // Validate the password length input
            int passwordLength;
            while (!int.TryParse(passwordLengthInput, out passwordLength) || passwordLength <= 0)
            {
                Console.WriteLine("无效的密码长度，请输入一个正整数.");
                Console.Write("请输入所需的密码长度: ");
                passwordLengthInput = Console.ReadLine();
            }

            // Prompt the user for the password type
            Console.WriteLine("请选择一个密码类型: ");
            Console.WriteLine("1. 纯数字");
            Console.WriteLine("2. 纯字母");
            Console.WriteLine("3. 数字加字母");
            Console.WriteLine("4. 数字加字母加符号");
            Console.Write("\n选择你所需要的密码类型: ");
            string passwordTypeInput = Console.ReadLine();

            // Generate the password based on the selected type
            string password = "";
            switch (passwordTypeInput)
            {
                case "1":
                    password = GenerateNumericPassword(passwordLength);
                    break;
                case "2":
                    password = GenerateAlphabeticPassword(passwordLength);
                    break;
                case "3":
                    password = GenerateAlphaNumericPassword(passwordLength);
                    break;
                case "4":
                    password = GenerateComplexPassword(passwordLength);
                    break;
                default:
                    Console.WriteLine("无效的选项。请重新运行程序并选择纯数字、纯字母、数字加字母或数字加字母加符号中的一个选项。");
                    Environment.Exit(0);
                    break;
            }

            // Print the password and save it to a file
            if (!string.IsNullOrEmpty(password))
            {
                Console.WriteLine("您的密码是: " + password);
                File.WriteAllText("pw.txt", password);
                Console.WriteLine("已将密码保存到pw.txt.");
            }
            Console.WriteLine("按任意键退出程序...");
            Console.ReadKey();
        }

        static string GenerateNumericPassword(int length)
        {
            const string validCharacters = "0123456789";
            return GeneratePassword(length, validCharacters);
        }

        static string GenerateAlphabeticPassword(int length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return GeneratePassword(length, validCharacters);
        }

        static string GenerateAlphaNumericPassword(int length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return GeneratePassword(length, validCharacters);
        }

        static string GenerateComplexPassword(int length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;':,.<>?";
            return GeneratePassword(length, validCharacters);
        }

        static string GeneratePassword(int length, string validCharacters)
        {
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
