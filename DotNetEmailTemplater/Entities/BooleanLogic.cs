using DotNetEmailTemplater.Interfaces;

namespace DotNetEmailTemplater.Entities
{
	/// <summary>
	/// Represents a portion of the code that can be shown or hidden
	/// based on the boolean value stored in the Value property.
	/// </summary>
	/// <example>
	/// {{#showMe}}Show this text is showMe is true{{/showMe}} If
	/// the showMe value was false, the text would not be displayed
	/// at all.
	/// </example>
	public class BooleanLogic : ITag
	{
		private string _name = string.Empty;
		/// <summary>
		/// Name provided for the BooleanObject tag.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _description = string.Empty;
		/// <summary>
		/// Optional description provided for the BooleanObject tag.
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private bool _value = false;
		/// <summary>
		/// Contains the value of the boolean statement.
		/// </summary>
		/// <remarks>
		/// Defaults to false, if not set.
		/// </remarks>
		public bool Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}
