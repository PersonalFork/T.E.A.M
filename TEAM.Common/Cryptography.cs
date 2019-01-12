using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TEAM.Common
{
    /// <summary>
    /// Cannot create instance and inherit this class.
    /// This class can be used to Encrypt and Decrypt any string value in base-64 bit format.
    /// </summary>
    public static class Cryptography
    {
        /// <summary>
        /// Encrypt input string value
        /// </summary>
        /// <param name="value">Input Value</param>
        /// <returns>Encrypted String</returns>
        #region
        public static string Encrypt(this string value)
        {
            return Encrypt(value, "ENCRYPT", "ENCRYPT", "MD5", 2, "FDS.FRAMEWORK.ADMIN", 256);
        }
        #endregion

        /// <summary>
        /// Decrypt input string value
        /// </summary>
        /// <param name="value">Input Value</param>
        /// <returns>Decrypted String</returns>
        #region
        public static string Decrypt(this string value)
        {
            return Decrypt(value, "ENCRYPT", "ENCRYPT", "MD5", 2, "FDS.FRAMEWORK.ADMIN", 256);
        }
        #endregion

        /// <summary> 
        /// Encrypts given user name and password for Service authentication
        /// </summary> 
        /// <param name="userName">User Name</param> 
        /// <param name="password">User password</param> 
        /// <returns>Encoded 64 bit string</returns> 
        #region
        public static string ConvertCredentialsToBase64String(string userName, string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format(CultureInfo.CurrentCulture, "{0}:{1}", userName, password)));
        }
        #endregion

        /// <summary>
        /// Encrypt string value
        /// </summary>
        /// <param name="text">Plain Text</param>
        /// <param name="passphrase">Pass Phrase</param>
        /// <param name="saltValue">Salt Value</param>
        /// <param name="hashAlgorithm">Algorithm Type</param>
        /// <param name="passwordIterations">Password Iterations</param>
        /// <param name="vector">Vector Value</param>
        /// <param name="keySize">Key Size</param>
        /// <returns>Encrypted String</returns>
        #region
        private static string Encrypt(
                                    string text,
                                    string passphrase,
                                    string saltValue,
                                    string hashAlgorithm,
                                    int passwordIterations,
                                    string vector,
                                    int keySize)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(vector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            string cipherText = string.Empty;

            using (PasswordDeriveBytes password = new PasswordDeriveBytes(
                passphrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations))
            {
                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    // It is reasonable to set encryption mode to Cipher Block Chaining
                    // (CBC). Use default options for other symmetric key parameters.
                    symmetricKey.Mode = CipherMode.CBC;

                    // Generate encryptor from the existing key bytes and initialization 
                    // vector. Key size will be defined based on the number of the key 
                    // bytes.
                    ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                        keyBytes,
                        initVectorBytes);

                    // Define memory stream which will be used to hold encrypted data.
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Define cryptographic stream (always use Write mode for encryption).
                        CryptoStream cryptoStream = new CryptoStream(
                            memoryStream,
                            encryptor,
                            CryptoStreamMode.Write);

                        // Start encrypting.
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                        // Finish encrypting.
                        cryptoStream.FlushFinalBlock();

                        // Convert our encrypted data from a memory stream into a byte array.
                        byte[] cipherTextBytes = memoryStream.ToArray();

                        // Convert encrypted data into a base64-encoded string.
                        cipherText = Convert.ToBase64String(cipherTextBytes);
                    }
                }
            }

            // Return encrypted string.
            return cipherText;
        }
        #endregion

        /// <summary>
        /// Decrypt string value
        /// </summary>
        /// <param name="text">Cipher Text</param>
        /// <param name="passphrase">Pass Phrase</param>
        /// <param name="saltValue">Salt Value</param>
        /// <param name="hashAlgorithm">Algorithm Type</param>
        /// <param name="passwordIterations">Password Iterations</param>
        /// <param name="initVector">Vector Value</param>
        /// <param name="keySize">Key Size</param>
        /// <returns>Decrypted string</returns>
        #region
        private static string Decrypt(
                                    string text,
                                    string passphrase,
                                    string saltValue,
                                    string hashAlgorithm,
                                    int passwordIterations,
                                    string initVector,
                                    int keySize)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(" ", "+");
            }

            byte[] cipherTextBytes = Convert.FromBase64String(text);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            string plainText = string.Empty;
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(
                passphrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations))
            {
                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    // It is reasonable to set encryption mode to Cipher Block Chaining
                    // (CBC). Use default options for other symmetric key parameters.
                    symmetricKey.Mode = CipherMode.CBC;

                    // Generate decryptor from the existing key bytes and initialization 
                    // vector. Key size will be defined based on the number of the key 
                    // bytes.
                    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                        keyBytes,
                        initVectorBytes);

                    // Define memory stream which will be used to hold encrypted data.
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        // Define cryptographic stream (always use Read mode for encryption).
                        CryptoStream cryptoStream = new CryptoStream(
                            memoryStream,
                            decryptor,
                            CryptoStreamMode.Read);

                        // Since at this point we don't know what the size of decrypted data
                        // will be, allocate the buffer long enough to hold ciphertext;
                        // plaintext is never longer than ciphertext.
                        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                        // Start decrypting.
                        int decryptedByteCount = cryptoStream.Read(
                            plainTextBytes,
                            0,
                            plainTextBytes.Length);

                        // Convert decrypted data into a string. 
                        // Let us assume that the original plaintext string was UTF8-encoded.
                        plainText = Encoding.UTF8.GetString(
                            plainTextBytes,
                            0,
                            decryptedByteCount);
                    }
                }
            }

            // Return decrypted string.   
            return plainText;
        }
        #endregion
    }
}
