using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AccountsModuleService
{
	/// <summary>
	/// Class Containing Function to Encrypt 
	/// and Decrypt Data using Different Encryption 
	/// Algorithm
	/// </summary>
	public class Encryption
	{
		#region Private Declarations 

		private string m_InternalHashKey		=	"$$##SERVION##$$"; //Hash Key Only for Internal Functions
		private string m_HashKey				=	"$#SERVION#$";	  // Hash Key based on Which the Password is Generated 
		private string m_saltValue				=	"s@1tValue";        // can be any string
		private string m_hashAlgorithm			=	"SHA1";             // can be "MD5"
		private int m_passwordIterations		=	2;                  // can be any number
		private string m_initVector				=	"@1B2c3D4e5F6g7H8"; // must be 16 bytes
		private int m_keySize					=	256;                // can be 192 or 128

		#endregion Private Declarations
		
		#region Constructor 
		/// <summary>
		/// Creates a New Instance of the Encryption Class
		/// </summary>
		public Encryption()
		{

		}
		

		#endregion Constructor 

		#region Public Functions 

		#region TripleDES
		/// <summary>
		/// Encrypt the Given Text Using TripleDES Algorithm
		/// </summary>
		/// <param name="TextToBeEncrypted">Text which is to be encrypted</param>
		/// <returns>Returns an Encrypted String </returns>
		public string EncryptUsingTripleDES( string TextToBeEncrypted ) 
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];
			
			// Create PasswordDeriveBytes object to derive key from password
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(m_HashKey, new byte[0]);
			
			// Algorithms can be "DES", "TripleDES", "RC2"
			// Hash can be "MD5", "SH1"

			des.Key = pdb.CryptDeriveKey( "RC2", "MD5", 128, new byte[8]);
			MemoryStream ms = new MemoryStream(TextToBeEncrypted.Length * 2);

			CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(),CryptoStreamMode.Write);
			
			byte[] plainBytes = Encoding.UTF8.GetBytes(TextToBeEncrypted);

			encStream.Write(plainBytes, 0, plainBytes.Length);

			encStream.FlushFinalBlock();
			byte[] encryptedBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(encryptedBytes, 0, (int)ms.Length);
			encStream.Close();
			return Convert.ToBase64String(encryptedBytes);

		}

		/// <summary>
		/// Encrypting the Given String using The TripleDES Algorithm with the specified HashKey
		/// </summary>
		/// <param name="TextToBeEncrypted">Text which is to be encrypted</param>
		/// <param name="Hashkey">Hash Key</param>
		/// <returns>Returns a Encrypted String </returns>
		public string EncryptUsingTripleDES ( string TextToBeEncrypted, string Hashkey )
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];
			
			// Create PasswordDeriveBytes object to derive key from password
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Hashkey, new byte[0]);
			
			// Algorithms can be "DES", "TripleDES", "RC2"
			// Hash can be "MD5", "SH1"

			des.Key = pdb.CryptDeriveKey( "RC2", "MD5", 128, new byte[8]);
			MemoryStream ms = new MemoryStream(TextToBeEncrypted.Length * 2);

			CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(),CryptoStreamMode.Write);
			
			byte[] plainBytes = Encoding.UTF8.GetBytes(TextToBeEncrypted);

			encStream.Write(plainBytes, 0, plainBytes.Length);

			encStream.FlushFinalBlock();
			byte[] encryptedBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(encryptedBytes, 0, (int)ms.Length);
			encStream.Close();
			return Convert.ToBase64String(encryptedBytes);

		}

		/// <summary>
		/// Decryptes a Given Text using TripleDES Algorithm
		/// </summary>
		/// <param name="TextToBeDecrypted">Text which is to be Decrypted</param>
		/// <returns>Returns a Plain Text After Decryption</returns>
		public string DecryptUsingTripleDES( string TextToBeDecrypted )
		{
			// Create Triple DES algorithm class object
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];

			// Create PasswordDeriveBytes object to derive key from password
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(m_HashKey, new byte[0]);
			des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
			
			// Convert Base64 Encrypted string to bytes
			byte[] encryptedBytes = Convert.FromBase64String( TextToBeDecrypted );

			// Create Memory stream object to pass the encrypted bytes to crypto stream
			MemoryStream ms = new MemoryStream(TextToBeDecrypted.Length);
			CryptoStream decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
			decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
			decStream.FlushFinalBlock();
			byte[] plainBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(plainBytes, 0, (int)ms.Length);
			decStream.Close();
			return Encoding.UTF8.GetString(plainBytes);
		}
		
		/// <summary>
		/// Decrypting the Given String using Hashkey specified - TripleDES Algorithm
		/// </summary>
		/// <param name="TextToBeDecrypted">Text which is to be Decrypted</param>
		/// <param name="HashKey">Hash key</param>
		/// <returns>Returns a Decrypted Plain Text String </returns>
		public string DecryptUsingTripleDES ( string TextToBeDecrypted , string HashKey )
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];

			// Create PasswordDeriveBytes object to derive key from password
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(HashKey, new byte[0]);
			des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
			
			// Convert Base64 Encrypted string to bytes
			byte[] encryptedBytes = Convert.FromBase64String( TextToBeDecrypted );

			// Create Memory stream object to pass the encrypted bytes to crypto stream
			MemoryStream ms = new MemoryStream(TextToBeDecrypted.Length);
			CryptoStream decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
			decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
			decStream.FlushFinalBlock();
			byte[] plainBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(plainBytes, 0, (int)ms.Length);
			decStream.Close();
			return Encoding.UTF8.GetString(plainBytes);
		}

		/// <summary>
		/// Encryptes the Given String Using TripleDES Algorithm 
		/// This is used only to encrypt Text that is used by the application internally 
		/// (e.g) Tpin is Encrypted using same algorithm but the Hash Key is different 
		/// as this will not be decrypted by the User any time as this is not exposed 
		/// </summary>
		/// <param name="TextToBeEncrypted">Text which is to be Encrypted</param>
		/// <returns>Returns a Encrypted Text </returns>
		public string EncryptUsingTripleDESForInternalUse ( string TextToBeEncrypted )
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.IV = new byte[8];
			
			// Create PasswordDeriveBytes object to derive key from password
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(m_InternalHashKey, new byte[0]);
			
			// Algorithms can be "DES", "TripleDES", "RC2"
			// Hash can be "MD5", "SH1"

			des.Key = pdb.CryptDeriveKey( "RC2", "MD5", 128, new byte[8]);
			MemoryStream ms = new MemoryStream(TextToBeEncrypted.Length * 2);

			CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(),CryptoStreamMode.Write);
			
			byte[] plainBytes = Encoding.UTF8.GetBytes(TextToBeEncrypted);

			encStream.Write(plainBytes, 0, plainBytes.Length);

			encStream.FlushFinalBlock();
			byte[] encryptedBytes = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(encryptedBytes, 0, (int)ms.Length);
			encStream.Close();
			return Convert.ToBase64String(encryptedBytes);
		}
		#endregion TripleDES

		#region Rijndael
		/// <summary>
		/// Encryptes the Given String Using Rijndael Algorithm
		/// </summary>
		/// <param name="TextToBeEncrypted">Text which is to be Encrypted</param>
		/// <returns>Returns a Encrypted Text</returns>
		public string EncryptUsingRijndael( string TextToBeEncrypted )
		{
			string TempEncryptString	=	RijndaelEncrypt( TextToBeEncrypted, 
				m_HashKey,m_saltValue, m_hashAlgorithm, m_passwordIterations,
				m_initVector, m_keySize );
										
			return TempEncryptString;											
		}
		/// <summary>
		/// Decryptes the Given String Using Rijndael Algorithm
		/// </summary>
		/// <param name="TextToBeDecrypted">Text which needs to be decrypted</param>
		/// <returns>Returns a Decrypted Plain Text</returns>
		public string DecryptUsingRijndael( string TextToBeDecrypted )
		{
			string TempPlainText		=	RijndaelDecrypt ( TextToBeDecrypted,
				m_HashKey, m_saltValue, m_hashAlgorithm, m_passwordIterations,
				m_initVector, m_keySize );
			return TempPlainText;
		}
		
		/// <summary>
		/// Encryptes the Given String Using Rijndael Algorithm with the Specified HashKey
		/// </summary>
		/// <param name="TextToBeEncrypted">Text which is to be Encrypted</param>
		/// <param name="Hashkey">Hash Key</param>
		/// <returns>Returns an Encrypted Text</returns>
		public string EncryptUsingRijndael( string TextToBeEncrypted , string Hashkey) 
		{
			string TempEncryptString	=	RijndaelEncrypt( TextToBeEncrypted, 
				Hashkey, m_saltValue, m_hashAlgorithm, m_passwordIterations,
				m_initVector, m_keySize );
										
			return TempEncryptString;	
		}
		/// <summary>
		/// Decryptes the Given String Using Rijndael Algorithm with the specified Hash Key
		/// </summary>
		/// <param name="TextToBeDecrypted">Text which is to be Decrypted</param>
		/// <param name="HashKey">Hash Key</param>
		/// <returns>Returns a Plain Text String After Decryption</returns>
		public string DecryptUsingRijndael( string TextToBeDecrypted , string HashKey )
		{
			string TempPlainText		=	RijndaelDecrypt ( TextToBeDecrypted,
				HashKey, m_saltValue, m_hashAlgorithm, m_passwordIterations,
				m_initVector, m_keySize );
			return TempPlainText;
		}

		/// <summary>
		/// Encryptes the Given String Using Rijndeal Algorithm 
		/// This is used only to encrypt Text that is used by the application internally 
		/// (e.g) Tpin is Encrypted using same algorithm but the Hash Key is different 
		/// as this will not be decrypted by the User any time as this is not exposed 
		/// </summary>
		/// <param name="TextToBeEncrypted">Text which is to be Encrypted</param>
		/// <returns>Returns a Encrypted Text</returns>
		public string EncryptUsingRijndaelForInternalUse ( string TextToBeEncrypted )
		{
			string TempEncryptString	=	RijndaelEncrypt( TextToBeEncrypted, 
				m_InternalHashKey ,m_saltValue, m_hashAlgorithm, m_passwordIterations,
				m_initVector, m_keySize );
										
			return TempEncryptString;	
		}
	
	
		#endregion Rijndael 

		#endregion Public Functions 

		#region Internal Methods 
		/// <summary>
		/// Encrypts specified plaintext using Rijndael symmetric key algorithm
		/// and returns a base64-encoded result.
		/// </summary>
		/// <param name="plainText">
		/// Plaintext value to be encrypted.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be 
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Encrypted value formatted as a base64-encoded string.
		/// </returns>
		private string RijndaelEncrypt(string plainText,
			string   passPhrase,
			string   saltValue,
			string   hashAlgorithm,
			int      passwordIterations,
			string   initVector,
			int      keySize)
		{
			// Convert strings into byte arrays.
			// Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);
        
			// Convert our plaintext into a byte array.
			// Let us assume that plaintext contains UTF8-encoded characters.
			byte[] plainTextBytes  = Encoding.UTF8.GetBytes(plainText);
        
			// First, we must create a password, from which the key will be derived.
			// This password will be generated from the specified passphrase and 
			// salt value. The password will be created using the specified hash 
			// algorithm. Password creation can be done in several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
				passPhrase, 
				saltValueBytes, 
				hashAlgorithm, 
				passwordIterations);
        
			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);
        
			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged();
        
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
			MemoryStream memoryStream = new MemoryStream();        
                
			// Define cryptographic stream (always use Write mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(memoryStream, 
				encryptor,
				CryptoStreamMode.Write);
			// Start encrypting.
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                
			// Finish encrypting.
			cryptoStream.FlushFinalBlock();

			// Convert our encrypted data from a memory stream into a byte array.
			byte[] cipherTextBytes = memoryStream.ToArray();
                
			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();
        
			// Convert encrypted data into a base64-encoded string.
			string cipherText = Convert.ToBase64String(cipherTextBytes);
        
			// Return encrypted string.
			return cipherText;
		}

		
		/// <summary>
		/// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
		/// </summary>
		/// <param name="cipherText">
		/// Base64-formatted ciphertext value.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Decrypted string value.
		/// </returns>
		/// <remarks>
		/// Most of the logic in this function is similar to the Encrypt
		/// logic. In order for decryption to work, all parameters of this function
		/// - except cipherText value - must match the corresponding parameters of
		/// the Encrypt function which was called to generate the
		/// ciphertext.
		/// </remarks>
		private string RijndaelDecrypt(string cipherText,
			string   passPhrase,
			string   saltValue,
			string   hashAlgorithm,
			int      passwordIterations,
			string   initVector,
			int      keySize)
		{
			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);
        
			// Convert our ciphertext into a byte array.
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        
			// First, we must create a password, from which the key will be 
			// derived. This password will be generated from the specified 
			// passphrase and salt value. The password will be created using
			// the specified hash algorithm. Password creation can be done in
			// several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
				passPhrase, 
				saltValueBytes, 
				hashAlgorithm, 
				passwordIterations);
        
			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);
        
			// Create uninitialized Rijndael encryption object.
			RijndaelManaged    symmetricKey = new RijndaelManaged();
        
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
			MemoryStream  memoryStream = new MemoryStream(cipherTextBytes);
                
			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream  cryptoStream = new CryptoStream(memoryStream, 
				decryptor,
				CryptoStreamMode.Read);

			// Since at this point we don't know what the size of decrypted data
			// will be, allocate the buffer long enough to hold ciphertext;
			// plaintext is never longer than ciphertext.
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        
			// Start decrypting.
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 
				0, 
				plainTextBytes.Length);
                
			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();
        
			// Convert decrypted data into a string. 
			// Let us assume that the original plaintext string was UTF8-encoded.
			string plainText = Encoding.UTF8.GetString(plainTextBytes, 
				0, 
				decryptedByteCount);
        
			// Return decrypted string.   
			return plainText;
		}
		#endregion Internal Methods 
	}
}

