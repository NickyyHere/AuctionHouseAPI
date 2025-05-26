namespace AuctionHouseAPI.Shared.Exceptions
{

	[Serializable]
	public class MinimumOutbidException : Exception
	{
		public MinimumOutbidException() { }
		public MinimumOutbidException(string message) : base(message) { }
		public MinimumOutbidException(string message, Exception inner) : base(message, inner) { }
		protected MinimumOutbidException(
		  System.Runtime.Serialization.SerializationInfo info,
			#pragma warning disable SYSLIB0051
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
