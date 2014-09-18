namespace PaintingCode.DotNetEmailTemplater.Entities
{
	/// <summary>
	/// Describes the elements of the email template object that this library expects.
	/// </summary>
	public interface IEmailTemplate
	{
		/// <summary>
		/// Subject to be used within the email.
		/// </summary>
		/// <returns></returns>
		string Subject { get; set; }

		/// <summary>
		/// The HTML form of the email.
		/// </summary>
		/// <returns></returns>
		string HtmlTemplate { get; set; }

		/// <summary>
		/// The non-HTML form of the email.
		/// </summary>
		/// <returns></returns>
		string TextTemplate { get; set; }
	}
}
