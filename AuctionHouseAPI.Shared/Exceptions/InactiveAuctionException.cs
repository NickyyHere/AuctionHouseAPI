namespace AuctionHouseAPI.Shared.Exceptions
{

	[Serializable]
	public class InactiveAuctionException : Exception
	{
		public InactiveAuctionException() { }
		public InactiveAuctionException(string message) : base(message) { }
		public InactiveAuctionException(string message, Exception inner) : base(message, inner) { }
		protected InactiveAuctionException(
		  System.Runtime.Serialization.SerializationInfo info,
			#pragma warning disable SYSLIB0051
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
