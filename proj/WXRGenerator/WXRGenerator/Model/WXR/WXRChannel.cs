namespace WXRGenerator.Model
{
	public class WXRChannel
	{
		public string Title { get; set; }
		public string Link { get; set; }
		public string Description { get; set; }
		public string PubDate { get; set; }
		public string Language { get; set; }
		public string Generator { get; set; }
		public string WXRVersion { get; set; }
		public string BaseSiteURL { get; set; }
		public string BaseBlogURL { get; set; }
		//public List<WXRCategory> Categories { get; set; }
		//public List<WXRTag> Tags { get; set; }
		public List<WXRAuthor> Authors { get; set; }
	}
}
