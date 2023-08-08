using CsvHelper.Configuration.Attributes;

namespace WXRGenerator.Model
{
	[Delimiter(",")]
	public class BlogPost
	{
		// title,link,pubDate,dc:creator,content:encoded,wp:post_id,wp:post_date,category
		[Name("title")]
		public string Title { get; set; }
		[Name("link")]
		public string Link { get; set; }
		[Name("pubDate")]
		public string PubDate { get; set; }
		[Name("dc:creator")]
		public string DcCreator { get; set; }
		[Name("content:encoded")]
		public string ContentEncoded { get; set; }
		[Name("wp:post_id")]
		public string WpPostId { get; set; }
		[Name("wp:post_date")]
		public string WpPostDate { get; set; }
		[Name("category")]
		public string Category { get; set; }

		// ToString() override
		public override string ToString()
		{
			return $"Title: {Title}, Link: {Link}, PubDate: {PubDate}, DcCreator: {DcCreator}, ContentEncoded: {ContentEncoded}, WpPostId: {WpPostId}, WpPostDate: {WpPostDate}, Category: {Category}";
		}
	}
}
