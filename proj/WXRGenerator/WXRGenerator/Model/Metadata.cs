using CsvHelper.Configuration.Attributes;

namespace WXRGenerator.Model
{
	[Delimiter(",")]
	public class Metadata
	{
		// title,link,description,language,wp:base_site_url,wp:base_blog_url,wp:author_id,wp:author_login,wp:author_email,wp:author_display_name,wp:author_first_name,wp:author_last_name

		[Name("title")]
		public string Title { get; set; }
		[Name("link")]
		public string Link { get; set; }
		[Name("description")]
		public string Description { get; set; }
		[Name("language")]
		public string Language { get; set; }
		[Name("wp:base_site_url")]
		public string WpBaseSiteUrl { get; set; }
		[Name("wp:base_blog_url")]
		public string WpBaseBlogUrl { get; set; }
		[Name("wp:author_id")]
		public string WpAuthorId { get; set; }
		[Name("wp:author_login")]
		public string WpAuthorLogin { get; set; }
		[Name("wp:author_email")]
		public string WpAuthorEmail { get; set; }
		[Name("wp:author_display_name")]
		public string WpAuthorDisplayName { get; set; }
		[Name("wp:author_first_name")]
		public string WpAuthorFirstName { get; set; }
		[Name("wp:author_last_name")]
		public string WpAuthorLastName { get; set; }

		// ToString() override
		public override string ToString()
		{
			return $"Title: {Title}, Link: {Link}, Description: {Description}, Language: {Language}, WpBaseSiteUrl: {WpBaseSiteUrl}, WpBaseBlogUrl: {WpBaseBlogUrl}, WpAuthorId: {WpAuthorId}, WpAuthorLogin: {WpAuthorLogin}, WpAuthorEmail: {WpAuthorEmail}, WpAuthorDisplayName: {WpAuthorDisplayName}, WpAuthorFirstName: {WpAuthorFirstName}, WpAuthorLastName: {WpAuthorLastName}";
		}
	}
}
