using System.Globalization;
using System.Text;
using System.Xml;

namespace WXRGenerator.Model.WXR
{
	public class WXR
	{
		public WXRChannel Channel { get; set; }
		public List<WXRItem> Items { get; set; }

		private WXR() { }

		public WXR(List<Metadata> metadata, List<BlogPost> blogPosts)
		{
			Channel = GenerateChannel(metadata);
			Items = GenerateItems(blogPosts);
		}

		private WXRChannel GenerateChannel(List<Metadata> metadataList)
		{
			var channel = new WXRChannel();
			var metadata = metadataList.First();

			channel.Title = metadata.Title;
			channel.Link = metadata.Link;
			channel.Description = metadata.Description;
			channel.PubDate = generatePubDate();
			channel.Language = metadata.Language;
			channel.Generator = "https://wordpress.org/?v=6.2.2";
			channel.WXRVersion = "1.2";
			channel.BaseSiteURL = metadata.WpBaseSiteUrl;
			channel.BaseBlogURL = metadata.WpBaseBlogUrl;

			var author = new WXRAuthor();
			author.AuthorId = metadata.WpAuthorId;
			author.AuthorLogin = metadata.WpAuthorLogin;
			author.AuthorEmail = metadata.WpAuthorEmail;
			author.AuthorDisplayName = metadata.WpAuthorDisplayName;
			author.AuthorFirstName = metadata.WpAuthorFirstName;
			author.AuthorLastName = metadata.WpAuthorLastName;
			channel.Authors = new List<WXRAuthor> { author };


			return channel;
		}

		private List<WXRItem> GenerateItems(List<BlogPost> blogPosts)
		{
			var items = new List<WXRItem>();

			foreach (var blogPost in blogPosts)
			{
				var item = new WXRItem();
				item.Title = blogPost.Title;
				item.Link = blogPost.Link;
				item.PubDate = blogPost.PubDate;
				item.Creator = blogPost.DcCreator;
				item.Guid = blogPost.Link;
				item.Description = string.Empty;
				item.ContentEncoded = blogPost.ContentEncoded;
				item.ExcerptEncoded = string.Empty;
				item.PostId = blogPost.WpPostId;
				item.PostDate = blogPost.WpPostDate;
				item.PostDateGmt = blogPost.WpPostDate;
				item.CommentStatus = "open";
				item.PingStatus = "open";
				item.Status = "publish";
				item.PostName = generatePostName(blogPost.DcCreator, blogPost.WpPostId);
				item.Category = blogPost.Category;

				items.Add(item);
			}

			return items;
		}

		private string generatePubDate()
		{
			// Tue, 08 Aug 2023 01:55:45 +0000
			// ddd should be English day of the week
			return DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss +0000",
				CultureInfo.CreateSpecificCulture("en-US"));
		}

		// Create a XML string represents WXR using the data from Channel and Items
		public string GenerateWXR()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.Encoding = Encoding.UTF8;

			XmlWriter xmlWriter = XmlWriter.Create("WXR.xml", settings);

			xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

			xmlWriter.WriteStartElement("rss");
			xmlWriter.WriteAttributeString("version", "2.0");
			xmlWriter.WriteAttributeString("xmlns", "excerpt", null, "http://wordpress.org/export/1.2/excerpt/");
			xmlWriter.WriteAttributeString("xmlns", "content", null, "http://purl.org/rss/1.0/modules/content/");
			xmlWriter.WriteAttributeString("xmlns", "wfw", null, "http://wellformedweb.org/CommentAPI/");
			xmlWriter.WriteAttributeString("xmlns", "dc", null, "http://purl.org/dc/elements/1.1/");
			xmlWriter.WriteAttributeString("xmlns", "wp", null, "http://wordpress.org/export/1.2/");
			string wp = "wp";

			xmlWriter.WriteStartElement("channel");

			xmlWriter.WriteElementString("title", Channel.Title);
			xmlWriter.WriteElementString("link", Channel.Link);
			xmlWriter.WriteElementString("description", Channel.Description);
			xmlWriter.WriteElementString("pubDate", Channel.PubDate);
			xmlWriter.WriteElementString("language", Channel.Language);
			xmlWriter.WriteElementString("generator", Channel.Generator);
			xmlWriter.WriteElementString("wxr_version", wp, Channel.WXRVersion);
			xmlWriter.WriteElementString("base_site_url", wp, Channel.BaseSiteURL);
			xmlWriter.WriteElementString("base_blog_url", wp, Channel.BaseBlogURL);
			// Write authors
			foreach (var author in Channel.Authors)
			{
				xmlWriter.WriteStartElement("author", wp);
				xmlWriter.WriteElementString("author_id", wp, author.AuthorId);
				insertCDATA(xmlWriter, "author_login", wp, author.AuthorLogin);
				insertCDATA(xmlWriter, "author_email", wp, author.AuthorEmail);
				insertCDATA(xmlWriter, "author_display_name", wp, author.AuthorDisplayName);
				insertCDATA(xmlWriter, "author_first_name", wp, author.AuthorFirstName);
				insertCDATA(xmlWriter, "author_last_name", wp, author.AuthorLastName);
				xmlWriter.WriteEndElement();
			}

			// Write items
			foreach (var item in Items)
			{
				xmlWriter.WriteStartElement("item");
				insertCDATA(xmlWriter, "title", item.Title);
				xmlWriter.WriteElementString("link", item.Link);
				xmlWriter.WriteElementString("pubDate", item.PubDate);
				insertCDATA(xmlWriter, "creator", item.Creator);
				xmlWriter.WriteElementString("guid", item.Guid);
				xmlWriter.WriteElementString("description", item.Description);
				
				xmlWriter.WriteStartElement("encoded", "content");
				xmlWriter.WriteRaw(item.ContentEncoded);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("encoded", "excerpt");
				xmlWriter.WriteRaw(item.ExcerptEncoded);
				xmlWriter.WriteEndElement();

				xmlWriter.WriteElementString("post_id", wp, item.PostId);
				insertCDATA(xmlWriter, "post_date", wp, item.PostDate);
				insertCDATA(xmlWriter, "post_date_gmt", wp, item.PostDateGmt);
				insertCDATA(xmlWriter, "comment_status", wp, item.CommentStatus);
				insertCDATA(xmlWriter, "ping_status", wp, item.PingStatus);
				insertCDATA(xmlWriter, "status", wp, item.Status);
				insertCDATA(xmlWriter, "post_name", wp, item.PostName);
				xmlWriter.WriteElementString("post_parent", wp, "0");
				xmlWriter.WriteElementString("menu_order", wp, "0");
				insertCDATA(xmlWriter, "post_type", wp, "post");
				//xmlWriter.WriteStartElement("post_password", wp, null);
				xmlWriter.WriteElementString("is_sticky", wp, "0");
				xmlWriter.WriteStartElement("category");
				xmlWriter.WriteAttributeString("domain", "category");
				xmlWriter.WriteAttributeString("nicename", item.Category);
				xmlWriter.WriteCData(item.Category);
				xmlWriter.WriteEndElement();
			}

			xmlWriter.WriteEndElement(); // channel
			xmlWriter.WriteEndElement(); // rss

			xmlWriter.Flush();
			var xmlString = xmlWriter.ToString();
			xmlWriter.Close();

			return xmlString;
		}

		private string generatePostName(string dcCreator, string postId)
		{
			return $"{dcCreator}-{postId}";
		}

		public void insertCDATA(XmlWriter xmlWriter, string element, string ns, string text)
		{
			if (string.IsNullOrEmpty(ns)) { 
				xmlWriter.WriteStartElement(element);
			}
			else { 
				xmlWriter.WriteStartElement(element, ns);
			}
			xmlWriter.WriteCData(text);
			xmlWriter.WriteEndElement();
		}

		public void insertCDATA(XmlWriter xmlWriter, string element, string text)
		{
			insertCDATA(xmlWriter, element, string.Empty, text);
		}
	}
}
