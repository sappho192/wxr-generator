namespace WXRGenerator.Model.WXR
{
	public class WXRItem
	{
		public string Title { get; set; }
		public string Link { get; set; }
		public string PubDate { get; set; }
		public string Creator { get; set; }
		public string Guid { get; set; }
		public string Description { get; set; }
		public string ContentEncoded { get; set; }
		public string ExcerptEncoded { get; set; }
		public string PostId { get; set; }
		public string PostDate { get; set; }
		public string PostDateGmt { get; set; }
		public string CommentStatus { get; set; }
		public string PingStatus { get; set; }
		public string Status { get; set; }
		public string PostName { get; set; }
		public string Category { get; set; }
		public string CategoryNice { get; set; }
		public List<WXRComment> Comments { get; set; }
	}
}
