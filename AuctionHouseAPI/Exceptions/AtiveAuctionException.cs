namespace AuctionHouseAPI.Exceptions
{

	[Serializable]
	public class ActiveAuctionException : Exception
	{
		public ActiveAuctionException() { }
		public ActiveAuctionException(string message) : base(message) { }
		public ActiveAuctionException(string message, Exception inner) : base(message, inner) { }
		protected ActiveAuctionException(
		  System.Runtime.Serialization.SerializationInfo info,
			#pragma warning disable SYSLIB0051
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
