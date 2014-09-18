using DotNetEmailTemplater.Entities;

namespace PaintingCode.DotNetEmailTemplater.Tests
{
	public class Template : IEmailTemplate
	{
		public string Title { get; set; }
		public string Subject { get; set; }
		public string HtmlTemplate { get; set; }
		public string TextTemplate { get; set; }
	}
}
