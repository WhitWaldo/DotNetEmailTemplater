using PaintingCode.DotNetEmailTemplater.Interfaces;

namespace PaintingCode.DotNetEmailTemplater.Entities
{
	public class StringVariable : ITag
	{
		private string _name = string.Empty;
		/// <summary>
		/// Name provied for the StringVariable tag.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _description = string.Empty;
		/// <summary>
		/// Optional description provided for the StringVariable tag.
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private string _value = string.Empty;
		/// <summary>
		/// Contains the value of the StringVariable tag.
		/// </summary>
		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}
