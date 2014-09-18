using System.Collections.Generic;

namespace PaintingCode.DotNetEmailTemplater.Entities
{
	public class EmailScope
	{
		public EmailScope()
		{
			HtmlEmailBody = string.Empty;
			PlainEmailBody = string.Empty;
			Subject = string.Empty;
			BooleanLogics = new List<BooleanLogic>();
			StringVariables = new List<StringVariable>();
		}

		private string _htmlEmailBody = string.Empty;
		/// <summary>
		/// Contains the HTML-formatted version of the email.
		/// </summary>
		public string HtmlEmailBody
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_htmlEmailBody))
					return "";
				return _htmlEmailBody;
			}
			set { _htmlEmailBody = value; }
		}

		private string _plainEmailBody = string.Empty;
		/// <summary>
		/// Contains the plain-text formatted version of the email.
		/// </summary>
		public string PlainEmailBody
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_plainEmailBody))
					return "";
				return _plainEmailBody;
			}
			set { _plainEmailBody = value; }
		}

		private string _subject = string.Empty;
		/// <summary>
		/// Contains the subject of the email.
		/// </summary>
		public string Subject
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_subject))
					return "";
				return _subject;
			}
			set { _subject = value; }
		}

		private List<string> _campaignTags = new List<string>();
		/// <summary>
		/// Contains a list of the campaign tags applicable to this email, if any.
		/// </summary>
		public List<string> CampaignTags
		{
			get { return _campaignTags; }
			set { _campaignTags = value; }
		}

		private List<BooleanLogic> _booleanLogics = new List<BooleanLogic>();
		/// <summary>
		/// Stores a List of each of the 
		/// BooleanLogics within an email body.
		/// </summary>
		public List<BooleanLogic> BooleanLogics
		{
			get { return _booleanLogics; }
			set { _booleanLogics = value; }
		}

		private List<StringVariable> _stringVariables = new List<StringVariable>();
		/// <summary>
		/// Stores a List of each of the 
		/// StringVariables within an email body.
		/// </summary>
		public List<StringVariable> StringVariables
		{
			get { return _stringVariables; }
			set { _stringVariables = value; }
		}
	}
}