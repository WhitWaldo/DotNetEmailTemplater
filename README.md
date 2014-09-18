DotNetEmailTemplater
====================

An email templating solution for .NET

Using this library
------------------

I recommend that you install the DotNetEmailTemplater via NuGet.  However, if you wish to build it yourself, there are no external dependencies,
so it should be as straightforward as cloning this repository, opening the solution within Visual Studio (I use 2013) and building it. Unit tests
are included.

Examples
========

If you would like a larger number of examples, check out the included unit tests as I test each of the functions within those.

Create an Email Template
------------------------

You'll need to create an object that makes use of the IEmailTemplate interface in order to make use of this library. You can do so using something similar to the following. If you should choose to skip any of the fields, this may or may not ultimately fail depending on how you're processing the fields for your email system, but the DotNetEmailTemplater will work whether the fields are populated or null. This tag substitution and interpretation will execute against all three properties.

		public class MyEmailTemplate : IEmailTemplate
		{
			public string Subject {get;set;}
			public string HtmlTemplate {get;set;}
			public string TextTemplate {get;set;}
		}

Boolean Tag
-----------

A boolean tag allows you to make use of a BooleanLogic object to either include the text contained within the open and close tags according
to the value of this object. If it's set to true, it will include the contained text and if not, it won't. A boolean tag takes the following form (open and close tags):

		{{#booleanTag}} some text to be included if tag is true {{/booleanTag}}
		
You can optionally include text within the opening tag that serves no purpose except to remind you in the future what the tag is supposed to contain, in case your name isn't descriptive enough. Simply include a pipe character after the name of the tag within the braces as in the following example:

		{{#booleanTag|This is a description of this boolean tag}} demonstration text {{/booleanTag}}
		
We'll look at one of the unit tests to see how this tag works:

		public void PopulateBooleanVariablesShouldYieldExpectedOutcome1()
		{
			const string input = "I {{#have}}was awesome{{/have}}{{#dontHave}}got tired{{/dontHave}}!";
			
			var variables = new List<BooleanLogic>
			{
				new BooleanLogic
				{
					Name = "have",
					Value = true
				},
				new BooleanLogic 
				{
					Name = "dontHave",
					Value = false
				}
			};
			
			var service = new Parser();
			var result = service.PopulateBooleanVariables(variables, input);
			
			Assert.AreEqual("I was awesome!", result);
		}
		
In this case, we have the possibility of two boolean tags within the input string (which could, of course, be populated in either the HTML or Text properties of the email). However, if you look at the instances of the BooleanLogic objects we create, we only set one of these to be true. With that in mind, when the input is parsed, the "was awesome" text is included since that value was 'true', but the "got tired" text is excluded because the value of that BooleanLogic object is 'false'.

Note that you can use these boolean tags more than once in the input string. The value of the BooleanLogic object will be used to determine the output across all like-named tags.


String Tag
----------

The string tag is merely a substitute for some other string that you wish to inject into the email string. The string tag takes the following form. You'll note that there is only a single tag and that it doesn't include an open and close tag like the boolean tag does.

		{{stringTag}}
		
As with the boolean tags, this tag can also contain documentation text after the tag name, provided it's precluded by a pipe character as in the following example:

		{{stringTag|This text provides a description of the tag}}

We'll look at another example from the unit tests to see how it's used:

		public void PopulateStringVariablesShouldYieldExpectedOutcome3()
		{
			const string input = "I {{modifier}} {{verb}} to the {{place}}.";
			var variables = new List<StringVariable>
			{
				new StringVariable
				{
					Name = "modified",
					Value = "usually"
				},
				new StringVariable
				{
					Name = "verb",
					Value = "hike"
				},
				new StringVariable
				{
					Name = "place",
					Value = "park"
				}
			}
			
			var service = new Parser();
			var result = service.PopulateStringVariables(variables, input);
			
			Assert.AreEqual("I usually hike to the park.", result);
		}

In this example, we can see that we have three string tags within this input. If you look at the instances of the StringVariable that we create, we simply indicate the name of the tag for each and the replacement value that each should contain. When the parser executes, it simply does a substitution of these values.

Combined Operation
------------------

Finally, we'll look at an example of using each pair of tags together within the same input string. Again, looking at one of the more complicated unit tests that makes use of string tags, boolean tags and descriptions within some of the tags. This example provides more of a real-world scenario as you'd typically only make use of the InitializeEmail() method in production. The other methods are intended for internal use, but are public for testing purposes.

		public void CombineVariablesTestWithDescriptions()
		{
			const string input =
				"Don't {{verb}} each {{#repeater}}and every {{/repeater}}{{time|Days or weeks}} by the {{noun}} you reap{{#includeRest|Includes the rest of the phrase}}, but by the {{otherNoun}}s that you plant{{/includeRest}}.";
			var template = new Template
			{
				Subject = "My {{verb}}",
				TextTemplate = input
			};
			
			var service = new Parser();
			
			var emailScope = service.InitializeEmail(template);
			
			emailScope.StringVariables.First(a => a.Name == "verb").Value = "judge";
			emailScope.BooleanLogics.First(a => a.Name == "repeater").Value = false;
			emailScope.StringVariables.First(a => a.Name == "time").Value = "day";
			emailScope.StringVariables.First(a => a.Name == "noun").Value = "harvest";
			emailScope.BooleanLogics.First(a => a.Name == "includeRest").Value = true;
			emailScope.StringVariables.First(a => a.Name == "otherNoun").Value = "seed";

			var output = service.FinalizeTemplate(emailScope);
			Assert.AreEqual("Don't judge each day by the harvest you reap, but by the seeds that you plant.", output.PlainEmailBody);
			Assert.AreEqual("My judge", output.Subject);
			Assert.IsTrue(emailScope.StringVariables.First(a => a.Name == "time").Description == "Days or weeks");
			Assert.IsTrue(emailScope.StringVariables.First(a => a.Name == "verb").Description == "");
			Assert.IsTrue(emailScope.BooleanLogics.First(a => a.Name == "includeRest").Description == "Includes the rest of the phrase");
			Assert.IsTrue(emailScope.BooleanLogics.First(a => a.Name == "repeater").Description == "");
		}
		
Note that we've got two tags within this example that make use of the descriptions. They're completely ignored by the interpreter as they intend to serve simply as comments as covered before. 

Otherwise, the interpreter substitutes the string variables as you'd expect with the text within the boolean tags included or excluded as indicated by the Value of the BooleanLogic objects that match by name.