using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetEmailTemplater.Entities;

namespace PaintingCode.DotNetEmailTemplater
{
	/// <summary>
	/// Performs the work needed to parse the emails to swap out template 
	/// text with the intended data. 
	/// </summary>
	public class Parser
	{
		/// <summary>
		/// Initially parses the email for each of the tags, then
		/// creates an email scope object for storing this information.
		/// </summary>
		/// <param name="template">Email template to parse.</param>
		/// <returns></returns>
		public EmailScope InitializeEmail(IEmailTemplate template)
		{
			//Iterate through the emailbody and identify each of the tags
			var result = new EmailScope
			{
				HtmlEmailBody = template.HtmlTemplate,
				PlainEmailBody = template.TextTemplate,
				Subject = template.Subject,
				BooleanLogics = new List<BooleanLogic>(),
				StringVariables = new List<StringVariable>()
			};

			var searchIndex = 0;
			//Run this once for the HTML body, again for the plain body and once more for the subject.
			if (!string.IsNullOrEmpty(template.HtmlTemplate))
			{
				while (searchIndex < template.HtmlTemplate.Length)
				{
					//Retrieve the index of the next occurence of {{ in the email
					var startIndexValue = GetIndexOfNextOpenTag(template.HtmlTemplate, searchIndex);
					if (startIndexValue == null)
					{
						//No more start tags
						searchIndex = template.HtmlTemplate.Length;
						break;
						//return result;
					}

					//Set the search index to this value now
					searchIndex = (int) startIndexValue;

					//Get the end index value of this tag
					var endIndexValue = GetIndexOfCloseTag(template.HtmlTemplate, searchIndex);

					//Verify that this tag has an ending
					if (endIndexValue == null)
					{
						//This tag doesn't actually end, let's bow out of this loop
						break;
					}

					//Now, set the search index to this value
					searchIndex = (int) endIndexValue;

					//Extract the contents of the tag
					var tagBody = template.HtmlTemplate.Substring((int) startIndexValue + 2,
						(int) endIndexValue - (int) startIndexValue - 2);

					//Set the values of the name and comment
					var tagName = tagBody;
					var tagDescription = "";

					//Identify if there's a comment in the tag
					if (tagBody.Contains("|"))
					{
						var splitValues = tagBody.Split('|');
						tagName = splitValues[0];
						tagDescription = splitValues[1];
					}

					//Look at the first character in the tag name (and see if it's a special character)
					if (tagName[0] == '#')
					{
						//This is a boolean logic tag
						var booleanTag = new BooleanLogic
						{
							Name = tagName.Substring(1), //Remove the # from the name
							Description = tagDescription
						};

						//Check to see that this tag doesn't already exist (name search)
						if (result.BooleanLogics.All(t => t.Name != booleanTag.Name))
						{
							//Append this tag to the email scope object
							result.BooleanLogics.Add(booleanTag);
						}
					}
					else if (tagName[0] == '/')
					{
						//This is a close tag for a boolean statement - ignore
					}
					else
					{
						//This is a variable tag
						var variableTag = new StringVariable()
						{
							Name = tagName,
							Description = tagDescription
						};

						//Check to see that this tag doesn't already exist (name match)
						if (result.StringVariables.All(t => t.Name != variableTag.Name))
						{
							//Append this tag to the email scope object
							result.StringVariables.Add(variableTag);
						}
					}
				}
			}

			//Now for the plain text body
			searchIndex = 0;
			if (!string.IsNullOrEmpty(template.TextTemplate))
			{
				while (searchIndex < template.TextTemplate.Length)
				{
					//Retrieve the index of the next occurence of {{ in the email
					var startIndexValue = GetIndexOfNextOpenTag(template.TextTemplate, searchIndex);
					if (startIndexValue == null)
					{
						//No more start tags
						searchIndex = template.TextTemplate.Length;
						break;
						//return result;
					}

					//Set the search index to this value now
					searchIndex = (int) startIndexValue;

					//Get the end index value of this tag
					var endIndexValue = GetIndexOfCloseTag(template.TextTemplate, searchIndex);

					//Verify that this tag has an ending
					if (endIndexValue == null)
					{
						//This tag doesn't actually end - let's bow out of this loop
						break;
					}

					//Now, set the search index to this value
					searchIndex = (int) endIndexValue;

					//Extract the contents of the tag
					var tagBody = template.TextTemplate.Substring((int) startIndexValue + 2,
						(int) endIndexValue - (int) startIndexValue - 2);

					//Set the values of the name and comment
					var tagName = tagBody;
					var tagDescription = "";

					//Identify if there's a comment in the tag
					if (tagBody.Contains("|"))
					{
						var splitValues = tagBody.Split('|');
						tagName = splitValues[0];
						tagDescription = splitValues[1];
					}

					//Look at the first character in the tag name (and see if it's a special character)
					if (tagName[0] == '#')
					{
						//This is a boolean logic tag
						var booleanTag = new BooleanLogic
						{
							Name = tagName.Substring(1), //Remove the # from the name
							Description = tagDescription
						};

						//Check to see that this tag doesn't already exist (name search)
						if (result.BooleanLogics.All(t => t.Name != booleanTag.Name))
						{
							//Append this tag to the email scope object
							result.BooleanLogics.Add(booleanTag);
						}
					}
					else if (tagName[0] == '/')
					{
						//This is a close tag for a boolean statement - ignore
					}
					else
					{
						//This is a variable tag
						var variableTag = new StringVariable()
						{
							Name = tagName,
							Description = tagDescription
						};

						//Check to see that this tag doesn't already exist (name match)
						if (result.StringVariables.All(t => t.Name != variableTag.Name))
						{
							//Append this tag to the email scope object
							result.StringVariables.Add(variableTag);
						}
					}
				}
			}

			//Finally for the subject
			searchIndex = 0;
			if (!string.IsNullOrEmpty(template.Subject))
			{
				while (searchIndex < template.Subject.Length)
				{
					//Retrieve the index of the next occurence of {{ in the email
					var startIndexValue = GetIndexOfNextOpenTag(template.Subject, searchIndex);
					if (startIndexValue == null)
					{
						//No more start tags
						searchIndex = template.Subject.Length;
						break;
						//return result;
					}

					//Set the search index to this value now
					searchIndex = (int) startIndexValue;

					//Get the end index value of this tag
					var endIndexValue = GetIndexOfCloseTag(template.Subject, searchIndex);

					//Verify that this tag has an ending
					if (endIndexValue == null)
					{
						//This tag doesn't actually end - let's bow out of this loop
						break;
					}

					//Now, set the search index to this value
					searchIndex = (int) endIndexValue;

					//Extract the contents of the tag
					var tagBody = template.Subject.Substring((int) startIndexValue + 2,
						(int) endIndexValue - (int) startIndexValue - 2);

					//Set the values of the name and comment
					var tagName = tagBody;
					var tagDescription = "";

					//Identify if there's a comment in the tag
					if (tagBody.Contains("|"))
					{
						var splitValues = tagBody.Split('|');
						tagName = splitValues[0];
						tagDescription = splitValues[1];
					}

					//Look at the first character in the tag name (and see if it's a special character)
					if (tagName[0] == '#')
					{
						//This is a boolean logic tag
						var booleanTag = new BooleanLogic
						{
							Name = tagName.Substring(1), //Remove the # from the name
							Description = tagDescription
						};

						//Check to see that this tag doesn't already exist (name search)
						if (result.BooleanLogics.All(t => t.Name != booleanTag.Name))
						{
							//Append this tag to the email scope object
							result.BooleanLogics.Add(booleanTag);
						}
					}
					else if (tagName[0] == '/')
					{
						//This is a close tag for a boolean statement - ignore
					}
					else
					{
						//This is a variable tag
						var variableTag = new StringVariable()
						{
							Name = tagName,
							Description = tagDescription
						};

						//Check to see that this tag doesn't already exist (name match)
						if (result.StringVariables.All(t => t.Name != variableTag.Name))
						{
							//Append this tag to the email scope object
							result.StringVariables.Add(variableTag);
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Retrieves the index of the next start tag.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="startPosition"></param>
		/// <returns>Index of the next tag start. If null, there are no remaining start tags.</returns>
		public int? GetIndexOfNextOpenTag(string input, int startPosition = 0)
		{
			if (startPosition >= input.Length)
				return null;

			//Add one to this since we would initially start ii at 1 instead of 0 (since we collect
			//the string test backwards
			startPosition++;

			for (var ii = startPosition; ii < input.Length - 1; ii++)
			{
				var chars = input.Substring(ii - 1, 2);
				if (chars == "{{")
				{
					return ii - 1;
				}
			}

			return null;
		}

		/// <summary>
		/// Retrieves the index of the end of the current start tag.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="startPosition"></param>
		/// <returns></returns>
		public int? GetIndexOfCloseTag(string input, int startPosition)
		{
			if (startPosition >= input.Length)
				return null;

			//Add one to this since we would initially start ii at 1 instead of 0 (since we collect
			//the string to test backwards.
			startPosition++;

			for (var ii = startPosition; ii < input.Length - 1; ii++)
			{
				var chars = input.Substring(ii - 1, 2);
				if (chars == "}}")
				{
					return ii - 1;
				}
			}

			return null;
		}

		/// <summary>
		/// Populates the email template according to the values of the 
		/// variables within the EmailScope.
		/// </summary>
		/// <param name="scope">The scope containing all the populated variables
		/// to insert into the template.</param>
		/// <returns></returns>
		public EmailScope FinalizeTemplate(EmailScope scope)
		{
			//First, let's build a list of each of the string variables that actually has a value now.
			var stringVarList = scope.StringVariables.Where(a => !string.IsNullOrWhiteSpace(a.Value)).ToList();

			//Now, go through the scope email body and populate all the string variables.
			var htmlBody = PopulateStringVariables(stringVarList, scope.HtmlEmailBody);
			var plainBody = PopulateStringVariables(stringVarList, scope.PlainEmailBody);
			var subject = PopulateStringVariables(stringVarList, scope.Subject);

			//Next, go through and populate all the boolean logics
			htmlBody = PopulateBooleanVariables(scope.BooleanLogics.ToList(), htmlBody);
			plainBody = PopulateBooleanVariables(scope.BooleanLogics.ToList(), plainBody);
			subject = PopulateBooleanVariables(scope.BooleanLogics.ToList(), subject);

			//Create the new email scope
			var newTemplateScope = new EmailScope
			{
				Subject = subject,
				HtmlEmailBody = htmlBody,
				PlainEmailBody = plainBody
			};

			//And we're done!
			return newTemplateScope;
		}

		/// <summary>
		/// Switch out the boolean logic from the email body.
		/// </summary>
		/// <param name="variables">The logic to be replacing.</param>
		/// <param name="emailBody">The body of the email.</param>
		/// <returns></returns>
		public string PopulateBooleanVariables(IEnumerable<BooleanLogic> variables, string emailBody)
		{
			var result = emailBody;
			foreach (var item in variables)
			{
				//Build the start tag of the boolean switch
				var sbStart = new StringBuilder();
				sbStart.Append("{{#");
				sbStart.Append(item.Name);
				if (!string.IsNullOrWhiteSpace(item.Description))
				{
					sbStart.Append("|");
					sbStart.Append(item.Description);
				}
				sbStart.Append("}}");
				var startTag = sbStart.ToString();

				//Build the end tag of the boolean switch
				var sbEnd = new StringBuilder();
				sbEnd.Append("{{/");
				sbEnd.Append(item.Name);
				sbEnd.Append("}}");
				var endTag = sbEnd.ToString();

				//If this tag is true, simply remove the tags from the message template
				if (item.Value)
				{
					result = result.Replace(startTag, "");
					result = result.Replace(endTag, "");
				}
				else
				{
					//If this tag is false, we need to both remove the tags as well as the content
					//in the middle of them.

					//Identify the index position of the start and end tags
					var startPosition = result.IndexOf(startTag, StringComparison.Ordinal);

					//The tag doesn't necessarily exist within here (since the tag might exclusively 
					//exist in another form of the email or the subject).
					if (startPosition == -1) continue;

					var endPosition = result.IndexOf(endTag, StringComparison.Ordinal);

					//Get the length of the characters between the (endPosition + length of endTag) and
					//the startPosition
					var removeLength = (endPosition + endTag.Length) - startPosition;

					result = result.Remove(startPosition, removeLength);
				}
			}
			return result;
		}

		/// <summary>
		/// Switch out the string variables from the email body.
		/// </summary>
		/// <param name="variables">The variables to be replacing.</param>
		/// <param name="emailBody">The body of the email.</param>
		/// <returns></returns>
		public string PopulateStringVariables(IEnumerable<StringVariable> variables, string emailBody)
		{
			var result = emailBody;
			foreach (var item in variables)
			{
				var sb = new StringBuilder();
				sb.Append("{{");
				sb.Append(item.Name);
				if (!string.IsNullOrWhiteSpace(item.Description))
				{
					sb.Append("|");
					sb.Append(item.Description);
				}
				sb.Append("}}");
				var variable = sb.ToString();

				result = result.Replace(variable, item.Value);
			}
			return result;
		}
	}
}