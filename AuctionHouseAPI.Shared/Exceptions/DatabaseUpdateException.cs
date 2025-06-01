
namespace AuctionHouseAPI.Shared.Exceptions
{

	[Serializable]
	public class DatabaseUpdateException : Exception
	{
		public DatabaseUpdateException() { }
		public DatabaseUpdateException(string message) : base(message) { }
		public DatabaseUpdateException(string message, Exception inner) : base(message, inner) { }
		protected DatabaseUpdateException(
		  System.Runtime.Serialization.SerializationInfo info,
#pragma warning disable SYSLIB0051
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
