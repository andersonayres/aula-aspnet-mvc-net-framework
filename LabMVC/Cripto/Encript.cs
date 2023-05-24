using System;
using System.Security.Cryptography;
using System.Text;

namespace LabMVC.Cripto
{
    public class Encript
    {
        public static string Encrypt(string text, string key)
        {
          
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            using (var aesAlg = new RijndaelManaged())
            {
                int requiredKeySizeInBytes = aesAlg.KeySize / 8; // Calcula o tamanho da chave em bytes
                byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Converte a chave para um array de bytes

                // Verifica se o tamanho da chave é válido
                if (keyBytes.Length != requiredKeySizeInBytes)
                {
                    // Se o tamanho da chave não for válido, ajuste-o para o tamanho correto
                    Array.Resize(ref keyBytes, requiredKeySizeInBytes);
                }
                                
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.CBC; // Usar o modo CBC
                aesAlg.Padding = PaddingMode.PKCS7; // Definir o modo de preenchimento desejado

                // Gerar um vetor de inicialização (IV)
                aesAlg.GenerateIV();
                byte[] iv = aesAlg.IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);

                // Concatenar o IV com os dados criptografados
                byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, combinedBytes, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

                return Convert.ToBase64String(combinedBytes);
            }
        }

        public static string Decrypt(string encryptedText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 32)
            {
                Array.Resize(ref keyBytes, 32);
            }
            byte[] combinedBytes = Convert.FromBase64String(encryptedText);

            using (var aesAlg = new RijndaelManaged())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.CBC; // Usar o modo CBC
                aesAlg.Padding = PaddingMode.PKCS7; // Definir o modo de preenchimento desejado

                // Extrair o IV do array combinado
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] encryptedBytes = new byte[combinedBytes.Length - iv.Length];
                Buffer.BlockCopy(combinedBytes, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

}
