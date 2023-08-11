namespace WXRGenerator.Model.WXR
{
	public class WXRComment
	{
		private WXRComment() { }
		public WXRComment(string commentId, string commentAuthor, string commentAuthorEmail, 
			string commentAuthorUrl, string commentAuthorIp, string commentDate, 
			string commentDateGmt, string commentContent, string commentApproved, 
			string commentType, string commentParent, string userId)
		{
			CommentId = commentId;
			CommentAuthor = commentAuthor;
			CommentAuthorEmail = commentAuthorEmail;
			CommentAuthorUrl = commentAuthorUrl;
			CommentAuthorIp = commentAuthorIp;
			CommentDate = commentDate;
			CommentDateGmt = commentDateGmt;
			CommentContent = commentContent;
			CommentApproved = commentApproved;
			CommentType = commentType;
			CommentParent = commentParent;
			UserId = userId;
		}

		//private static int _commentId = 1;
		public string CommentId { get; set; }
		/// <summary>
		/// Use insertCDATA() or writeCData() to wrap the value in CDATA tags
		/// </summary>
		public string CommentAuthor { get; set; }
		public string CommentAuthorEmail { get; set; }
		public string CommentAuthorUrl { get; set; }
		public string CommentAuthorIp { get; set; }
		public string CommentDate { get; set; }
		public string CommentDateGmt { get; set; }
		/// <summary>
		/// Use insertCDATA() or writeCData() to wrap the value in CDATA tags
		/// </summary>
		public string CommentContent { get; set; }
		public string CommentApproved { get; set; }
		public string CommentType { get; set; }
		public string CommentParent { get; set; }
		public string UserId { get; set; }
	}
}