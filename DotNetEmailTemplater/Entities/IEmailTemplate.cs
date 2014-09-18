namespace DotNetEmailTemplater.Entities
{
	/// <summary>
	/// Describes the elements of the email template object that this library expects.
	/// </summary>
	public interface IEmailTemplate
	{
		/// <summary>
		/// The user-friendly name for the email template.
		/// </summary>
		/// <returns></returns>
		string Title { get; set; }

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
