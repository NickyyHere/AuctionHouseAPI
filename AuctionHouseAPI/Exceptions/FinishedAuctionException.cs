namespace AuctionHouseAPI.Exceptions
{

	[Serializable]
	public class FinishedAuctionException : Exception
	{
		public FinishedAuctionException() { }
		public FinishedAuctionException(string message) : base(message) { }
		public FinishedAuctionException(string message, Exception inner) : base(message, inner) { }
		protected FinishedAuctionException(
		  System.Runtime.Serialization.SerializationInfo info,
			#pragma warning disable SYSLIB0051
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
