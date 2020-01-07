using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using System.Security;

namespace SecureStringPoc.Tests
{
	/// <summary>
	/// Notes:
	/// 
	/// Clean Code: Chapter 8, p. 116
	/// This is a Learning Test: Exploring and Learning Boundaries
	/// 
	/// https://docs.microsoft.com/en-us/dotnet/api/system.security.securestring?redirectedfrom=MSDN&view=netframework-4.8#HowSecure
	/// A SecureString object should never be constructed from a String, 
	/// because the sensitive data is already subject to the memory persistence
	/// consequences of the immutable String class. The best way to construct a 
	/// SecureString object is from a character-at-a-time unmanaged source, 
	/// such as the Console.ReadKey method.
	/// </summary>
	[TestClass]
	public class SecureStringTest
	{
		[TestMethod]
		public void SecureString_ShouldBeCreated_FromString()
		{
			//arrange
			var secureString = new SecureString();
			var secret = "Secret";

			//act
			secret.ToCharArray().ToList().ForEach(character => secureString.AppendChar(character));

			//assert
			Assert.IsNotNull(secureString);
		}

		[TestMethod]
		public void SecureString_ShouldBeCreated_FromNetworkCredential()
		{
			//act
			var secureString = new NetworkCredential("", "Secret").SecurePassword;

			//assert
			Assert.IsNotNull(secureString);

		}

		[TestMethod]
		public void SecureString_ShouldBeReadable_AsAString()
		{
			//arrange
			var secureString = new SecureString();
			"Secret".ToCharArray().ToList().ForEach(character => secureString.AppendChar(character));

			//act
			//NOTE: never do this
			string secret = new NetworkCredential(string.Empty, secureString).Password;

			//assert
			Assert.AreEqual("Secret", secret);
		}

		[TestMethod]
		public void SecureString_ShouldBeDisposed()
		{
			//arrange
			var secureString = new SecureString();
			"Secret".ToCharArray().ToList().ForEach(character => secureString.AppendChar(character));

			//act
			//NOTE: Disposing of an object does not release the object from memory.
			secureString.Dispose();

			try
			{
				string secret = new System.Net.NetworkCredential(string.Empty, secureString).Password;
			}
			catch (System.ObjectDisposedException)
			{
				//assert
				Assert.IsTrue(true);
			}
		}
	}
}
