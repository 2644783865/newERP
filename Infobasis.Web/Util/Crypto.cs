using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using Infobasis.Web;
using Infobasis.Web.Util;
using Infobasis.Web;

namespace Infobasis.Web.Util.Cryptography
{
	/// <summary>
	/// Summary description for Crypto.
	/// </summary>
	public static class Crypto
	{
		const string HASH_ALGORITHM = "SHA1";
		const string CRYPT_ALGORITHM = "DES";

		const char PLUS_ALTERNATE_CHAR = '@';
		const char FORWARD_SLASH_ALTERNATE_CHAR = '$';


        static Crypto()
        {
            string key = Global.Config["QueryStringEncryptionKey"];
            if (key == null)
                key = "15F6E490F59C2C79"; //Default key, should not be used in production. That would be naughty.
            InitCryptAlgorithm(key);
        }

		public enum EncryptionMode
		{
			/// <summary>
			/// Applies a randomisation preocess to the data prior to encryption, ensuring that the same plaintext can be 
			/// encrypted to one of a number of different ciphertext values
			/// </summary>
			Randomised,
			/// <summary>
			/// Ensures that the same plaintext always becomes the same ciphertext
			/// </summary>
			Repeatable
		}


		public static bool IsEncryptionEnabled
		{
			get
			{
				return _cryptAlgorithm != null;
			}
		}

		/// <summary>
		/// Computes a hash for the given input string using the SHA1 algorithm.
		/// </summary>
		/// <param name="str">String to for which to compute hash.</param>
		/// <returns>Base-64 encoded hash string.</returns>
		public static string ComputeHash(this string str)
		{
			if (str == null)
				throw new ArgumentNullException("str");

			byte[] bytes = Encoding.UTF8.GetBytes(str);

			HashAlgorithm hashAlgorithm = HashAlgorithm.Create(HASH_ALGORITHM);
			byte[] hash = hashAlgorithm.ComputeHash(bytes);
			hashAlgorithm.Clear(); // dispose

			return Convert.ToBase64String(hash);
		}

		static SymmetricAlgorithm _cryptAlgorithm;

        public static void InitCryptAlgorithm(string keyHexString)
		{
            if (keyHexString == null || keyHexString.Length == 0)
            {
                _cryptAlgorithm = null;
                return; // encryption off
            }

            byte[] cryptKey = StringUtil.HexStringToByteArray(keyHexString);

			_cryptAlgorithm = SymmetricAlgorithm.Create(CRYPT_ALGORITHM);

			if (!_cryptAlgorithm.ValidKeySize(cryptKey.Length * 8))
			{
				string msg =
					"Invalid Configuration value \"QueryStringEncryptionKey\". " +
					"The specified key size of " + cryptKey.Length + " bytes is incorrect for the \"" + CRYPT_ALGORITHM + "\" algorithm. " +
					"Try " + _cryptAlgorithm.Key.Length + " bytes.";
				throw new ApplicationException(msg);
			}

			// Set key and clear IV
			_cryptAlgorithm.Key = cryptKey;
			_cryptAlgorithm.IV = new byte[_cryptAlgorithm.IV.Length];

            _cryptAlgorithm.Padding = PaddingMode.ISO10126; // using ISO padding introduces randomness to the padding bytes, preventing padding oracle probes from guessing at what correct padding is decrypting to.
		}


		//=================================================================================================
		/// <summary>
		/// Encrypts a string using the QueryStringEncryptionKey from ib.config into a format that doesn't need any special  
		/// encoding to be used in a URL (a version of Base64 that uses chars "-." instead of "+/" respectively).
		/// The result is wrapped in parentheses to indicate that it is an encrypted value.
		/// </summary>
		/// <param name="plainText">String to be encrypted.</param>
		/// <returns></returns>
		public static string Encrypt(this string plainText)
		{
			return Encrypt(plainText, EncryptionMode.Randomised);
		}


		/// <summary>
		/// Encrypts a string using the QueryStringEncryptionKey from ib.config into a format that doesn't need any special  
		/// encoding to be used in a URL (a version of Base64 that uses chars "-." instead of "+/" respectively).
		/// The result is wrapped in parentheses to indicate that it is an encrypted value.
		/// </summary>
		/// <param name="plainText">String to be encrypted.</param>
		/// <returns></returns>
		public static string Encrypt(this string plainText, EncryptionMode encryptionMode)
		{
			if (plainText == null)
				throw new ArgumentNullException("plainText");

			// If encryption is off then passthrough
			if (_cryptAlgorithm == null)
				return plainText;

			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] encBytes = Encrypt(bytes, encryptionMode);
			return ToUrlFriendlyBase64(encBytes);
		}

		/// <summary>
		/// Decrypts data encrypted using <see cref="Encrypt"/> using the QueryStringEncryptionKey from ib.config.
		/// </summary>
		/// <param name="cipherText">String to be decrypted. Must be wrapped in parentheses "( )".</param>
		/// <returns></returns>
		public static string Decrypt(this string cipherText)
		{
			if (cipherText == null)
				throw new ArgumentNullException("cipherText");
			if (cipherText.Length == 0)
				throw new ArgumentException("Argument 'cipherText' cannot be empty string.");

			// If encryption is off then passthrough
			if (_cryptAlgorithm == null)
				return cipherText;


			byte[] encBytes = FromUrlFriendlyBase64(cipherText);

			byte[] plainBytes;
			try
			{
				plainBytes = Decrypt(encBytes);
			}
			catch (CryptographicException cryptoError)
			{
				throw new ArgumentException("Argument \"" + cipherText + "\" was invalid for decryption. " +
					"Possible causes: decryption key not the same as encryption key (check IB.config QueryStringEncryptionKey setting); or the string to decrypt was damaged or truncated.",
					"cipherText", cryptoError);
			}

			string plainText = Encoding.UTF8.GetString(plainBytes);

			return plainText;
		}

		// Creates a Base-64 string from binary data that uses '-' instead of '+' and '.' instead of '/' and doesn't use 
		// any '=' padding, so that it doesn't require any URL-encoding to be used in a URL. 
		// The result is prefixed and suffixed with a marker characters '(' and ')' to indicate it's an encrypted string.
		public static string ToUrlFriendlyBase64(this byte[] bytes)
		{
			// NOTE Convert.ToBase64CharArray() has an undocumented bug where the offsetOut 
			// has no effect, so that's why we don't use it here
			char[] base64Chars = Convert.ToBase64String(bytes).ToCharArray();

			// Convert:
			//   '+' to '-'
			//   '/' to '.' 
			//   Truncate '=' padding at end
			int i = 0;
			for (; i < base64Chars.Length; i++)
			{
				char ch = base64Chars[i];

				if (ch == '+')
					base64Chars[i] = PLUS_ALTERNATE_CHAR;

				else if (ch == '/')
					base64Chars[i] = FORWARD_SLASH_ALTERNATE_CHAR;

				else if (ch == '=')
					break; // start of Base64 padding, truncate here
			}
			// return base64 up to first '=' padding char, prefixed with '!'
			return new string(base64Chars, 0, i);
		}


        public static bool IsValidEncryptedFormat(this string str)
		{
			if (str == null || str.Length < 12)
				return false;

			for (int i = 1; i < str.Length - 1; i++)
			{
				char ch = str[i];
				if (!(
					(ch >= '0' && ch <= '9')
					|| (ch >= 'A' && ch <= 'Z')
					|| (ch >= 'a' && ch <= 'z')
					|| ch == PLUS_ALTERNATE_CHAR
					|| ch == FORWARD_SLASH_ALTERNATE_CHAR
					)
					)
					return false;
			}
			return true;
		}


        // Recreates binary data created using toUrlFriendlyBase64() method.
		public static byte[] FromUrlFriendlyBase64(string base64String)
		{
			if (base64String == null)
				throw new ArgumentNullException("str");

			int stringLength = base64String.Length;

			if (stringLength == 0)
				throw new ArgumentException("Argument cannot be empty string.", "str");

			if (!IsValidEncryptedFormat(base64String))
				throw new FormatException("Argument \"base64String\" invalid. Value was \"" + base64String + "\"");

			if (stringLength < 4)
				throw new FormatException("Argument base64String must be at least 4 characters. Value \"" + base64String + "\" is too short.");

			int paddingLength = 4 - (stringLength % 4);
			if (paddingLength == 4)
				paddingLength = 0;

			char[] b64chars = new char[stringLength + paddingLength];

			// Convert:
			//   '-' to '+'
			//   '.' to '/' 
			//   Pad end with '='
			for (int i = 0; i < b64chars.Length; i++)
			{
				if (i < stringLength)
				{
					char ch = base64String[i];
					if (ch == PLUS_ALTERNATE_CHAR)
						b64chars[i] = '+';
					else if (ch == FORWARD_SLASH_ALTERNATE_CHAR)
						b64chars[i] = '/';
					else
						b64chars[i] = ch;
				}
				else // add padding
				{
					b64chars[i] = '=';
				}
			}

			return Convert.FromBase64CharArray(b64chars, 0, b64chars.Length);
		}


		static Random _random = new Random();


        /// <summary>
		/// Encrypts binary data using the QueryStringEncryptionKey from ib.config.
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] plainText)
		{
			return Encrypt(plainText, EncryptionMode.Randomised);
		}

		/// <summary>
		/// Encrypts binary data using the QueryStringEncryptionKey from ib.config.
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] plainText, EncryptionMode encryptionMode)
		{

			// If encryption is off then passthrough
			if (_cryptAlgorithm == null)
				return plainText;

			using (new CumulativeTraceTimer("Encrypting", "Crypto", false))
			{

				// First we "jumble" the data slightly to make known ciphertexts difficult to recognise
				// This is a poor man's IV (Initialisation Vector). We don't use IVs because they almost
				// double the size of our ciphertext (for small data such as IbPrimaryKeys).
				byte randomisingByte = (byte)0x00;

				if (encryptionMode == EncryptionMode.Repeatable)
				{
					// derive a randomising byte from the message itself
					// we combine the lower bits of alternate bytes into the upper and lower half of the 
					// randomising byte.
					for (int i = 0; i < plainText.Length; i += 2)
					{
						randomisingByte ^= (byte)((plainText[i] & (byte)0x0f) << 4);
						if (i + 1 < plainText.Length)
						{
							randomisingByte ^= (byte)(plainText[i + 1] & (byte)0x0f);
						}
					}
				}
				else
				{
					randomisingByte = (byte)_random.Next(0, 255);
				}
				for (int i = 0; i < plainText.Length; i++)
					plainText[i] = (byte)((int)plainText[i] ^ (int)randomisingByte);

				// Perform the encryption
				ICryptoTransform encryptor = getPooledCryptoTransform(true);
				byte[] encryptedBytes = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
				returnCryptoTransformToPool(encryptor, true);

				// Concatenate the randomising byte and the encrypted bytes
				byte[] outputBytes = new byte[encryptedBytes.Length + 1];
				outputBytes[0] = randomisingByte;
				Buffer.BlockCopy(encryptedBytes, 0, outputBytes, 1, encryptedBytes.Length);
				return outputBytes;
			}
		}

		//=======================================================================
		/// <summary>
		/// Decrypts binary data using the QueryStringEncryptionKey from ib.config.
		/// </summary>
		/// <param name="cipherText"></param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] cipherText)
		{
			// If encryption is off then passthrough
			if (_cryptAlgorithm == null)
				return cipherText;

			using (new CumulativeTraceTimer("Decrypting", "Crypto", false))
			{
				// We decrypt the data, remembering to skip the 1st byte which is the randomising byte
				ICryptoTransform decryptor = getPooledCryptoTransform(false);
				byte[] plainText = decryptor.TransformFinalBlock(cipherText, 1, cipherText.Length - 1);
				returnCryptoTransformToPool(decryptor, false);

				// Now "un-jumble" the data by XORing the data with the randomising byte
				int randomisingByte = (int)cipherText[0];
				for (int i = 0; i < plainText.Length; i++)
					plainText[i] = (byte)((int)plainText[i] ^ randomisingByte);

				return plainText;
			}
		}



		//=================================================================================================
		// Crypto Pooling 
		//=================================================================================================
		static Stack _encryptorStack = new Stack();
		static Stack _decryptorStack = new Stack();
		const int MAX_CRYPTO_POOL_SIZE = 100;

		//=================================================================================================
		static ICryptoTransform getPooledCryptoTransform(bool isEncryptor)
		{
			Stack stack = isEncryptor ? _encryptorStack : _decryptorStack;

			lock (stack.SyncRoot)
			{
				// If nothing in the stack then create a new one, otherwise return a pooled one
				if (stack.Count == 0)
					return isEncryptor ? _cryptAlgorithm.CreateEncryptor() : _cryptAlgorithm.CreateDecryptor();
				else
					return (ICryptoTransform)stack.Pop();
			}
		}

		//=================================================================================================
		static void returnCryptoTransformToPool(ICryptoTransform cryptoTransform, bool isEncryptor)
		{
			if (cryptoTransform.CanReuseTransform)
			{
				Stack stack = isEncryptor ? _encryptorStack : _decryptorStack;

				// Return to pool if we haven't reached the maximum size
				lock (stack.SyncRoot)
				{
					if (stack.Count < MAX_CRYPTO_POOL_SIZE)
						stack.Push(cryptoTransform);
				}
			}
		}


	}
}
