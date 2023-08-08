using System.Globalization;
using System.Text;
using System.Xml;

namespace WXRGenerator.Model.WXR
{
	public class WXR
	{
		public WXRChannel Channel { get; set; }
		public List<WXRItem> Items { get; set; }
		private string timezone { get; set; }

		private WXR() { }

		public WXR(List<Metadata> metadata, List<BlogPost> blogPosts)
		{
			Channel = GenerateChannel(metadata);
			timezone = metadata.First().Timezone;
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
				item.PostDateGmt = toGMTDate(blogPost.WpPostDate);
				item.CommentStatus = "open";
				item.PingStatus = "open";
				item.Status = "publish";
				item.PostName = generatePostName(blogPost.DcCreator, blogPost.WpPostId);
				item.Category = blogPost.Category;

				items.Add(item);
			}

			return items;
		}

		private string toGMTDate(string wpPostDate)
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(wpPostDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
			{ 
				var modifiedDateTime = dateTime.AddHours(-int.Parse(timezone));
				return modifiedDateTime.ToString("yyyy-MM-dd HH:mm");
			} else
			{
				return wpPostDate;
			}
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
			string dc = "dc";

			xmlWriter.WriteStartElement("channel");

			xmlWriter.WriteElementString("title", Channel.Title);
			xmlWriter.WriteElementString("link", Channel.Link);
			xmlWriter.WriteElementString("description", Channel.Description);
			xmlWriter.WriteElementString("pubDate", Channel.PubDate);
			xmlWriter.WriteElementString("language", Channel.Language);
			xmlWriter.WriteElementString("generator", Channel.Generator);
			xmlWriter.WriteElementString(prefix: wp, "wxr_version", null, Channel.WXRVersion);
			xmlWriter.WriteElementString(prefix: wp, "base_site_url", null, Channel.BaseSiteURL);
			xmlWriter.WriteElementString(prefix: wp, "base_blog_url", null, Channel.BaseBlogURL);
			// Write authors
			foreach (var author in Channel.Authors)
			{
				xmlWriter.WriteStartElement(prefix: wp, "author", null);
				xmlWriter.WriteElementString(prefix: wp, "author_id", null, author.AuthorId);
				insertCDATA(xmlWriter, wp, "author_login", author.AuthorLogin);
				insertCDATA(xmlWriter, wp, "author_email", author.AuthorEmail);
				insertCDATA(xmlWriter, wp, "author_display_name", author.AuthorDisplayName);
				insertCDATA(xmlWriter, wp, "author_first_name", author.AuthorFirstName);
				insertCDATA(xmlWriter, wp, "author_last_name", author.AuthorLastName);
				xmlWriter.WriteEndElement();
			}

			// Write items
			foreach (var item in Items)
			{
				xmlWriter.WriteStartElement("item");
				{
					insertCDATA(xmlWriter, "title", item.Title);
					xmlWriter.WriteElementString("link", item.Link);
					xmlWriter.WriteElementString("pubDate", item.PubDate);
					insertCDATA(xmlWriter, dc, "creator", item.Creator);
					xmlWriter.WriteStartElement("guid");
					xmlWriter.WriteAttributeString("isPermaLink", "false");
					xmlWriter.WriteString(item.Guid);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteElementString("description", item.Description);

					xmlWriter.WriteStartElement(prefix: "content", "encoded", null);
					xmlWriter.WriteRaw(item.ContentEncoded);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement(prefix: "excerpt", "encoded", null);
					xmlWriter.WriteRaw(item.ExcerptEncoded);
					xmlWriter.WriteEndElement();

					xmlWriter.WriteElementString(prefix: wp, "post_id", null, item.PostId);
					insertCDATA(xmlWriter, wp, "post_date", item.PostDate);
					insertCDATA(xmlWriter, wp, "post_date_gmt", item.PostDateGmt);
					insertCDATA(xmlWriter, wp, "post_modified", item.PostDate);
					insertCDATA(xmlWriter, wp, "post_modified_gmt", item.PostDateGmt);
					insertCDATA(xmlWriter, wp, "comment_status", item.CommentStatus);
					insertCDATA(xmlWriter, wp, "ping_status", item.PingStatus);
					insertCDATA(xmlWriter, wp, "status", item.Status);
					insertCDATA(xmlWriter, wp, "post_name", item.PostName);
					xmlWriter.WriteElementString(prefix: wp, "post_parent", null, "0");
					xmlWriter.WriteElementString(prefix: wp, "menu_order", null, "0");
					insertCDATA(xmlWriter, wp, "post_type", "post");
					xmlWriter.WriteElementString(prefix: wp, "post_password", null, null);
					xmlWriter.WriteElementString(prefix: wp, "is_sticky", null, "0");
					xmlWriter.WriteStartElement("category");
					xmlWriter.WriteAttributeString("domain", "category");
					xmlWriter.WriteAttributeString("nicename", item.Category);
					xmlWriter.WriteCData(item.Category);
					xmlWriter.WriteEndElement();
				}
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

		public void insertCDATA(XmlWriter xmlWriter, string prefix, string element, string text)
		{
			if (string.IsNullOrEmpty(prefix))
			{
				xmlWriter.WriteStartElement(element);
			}
			else
			{
				xmlWriter.WriteStartElement(prefix: prefix, element, null);
			}
			xmlWriter.WriteCData(text);
			xmlWriter.WriteEndElement();
		}

		public void insertCDATA(XmlWriter xmlWriter, string element, string text)
		{
			insertCDATA(xmlWriter, string.Empty, element, text);
		}
	}
}
