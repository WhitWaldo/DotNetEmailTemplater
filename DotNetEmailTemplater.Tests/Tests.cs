using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaintingCode.DotNetEmailTemplater.Entities;

namespace PaintingCode.DotNetEmailTemplater.Tests
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void ShouldReturnASingleBooleanLogic()
		{
			const string input = "In the beginning, there was {{#ifGreaterThanOne}}one and{{/ifGreaterThanOne}} only one subject";
			var template = new Template
			{
				TextTemplate = input
			};

			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 1);
			Assert.IsTrue(result.StringVariables.Count == 0);
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifGreaterThanOne"));
		}

		[TestMethod]
		public void ShouldReturnTwoBooleanLogics()
		{
			const string input =
				"In the {{#ifGreater}}beginning{{/ifGreater}}{{#ifLesser}}end{{/ifLesser}}, there was something.";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 2);
			Assert.IsTrue(result.StringVariables.Count == 0);
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifGreater"));
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifLesser"));
		}

		[TestMethod]
		public void BooleanVariablesShouldSupportDescriptions()
		{
			const string input = "I {{#have|Random Description}}was awesome{{/have}}!";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.StringVariables.Count == 0);
			Assert.IsTrue(result.BooleanLogics.Count == 1);
			Assert.IsTrue(result.BooleanLogics[0].Description == "Random Description");
			Assert.IsTrue(result.BooleanLogics[0].Name == "have");
		}

		[TestMethod]
		public void ShouldReturnAStringVariable()
		{
			const string input = "Hey, {{firstName}}! What's up?";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 0);
			Assert.IsTrue(result.StringVariables.Count == 1);
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "firstName"));
		}

		[TestMethod]
		public void ShouldReturnTwoVariables()
		{
			const string input = "So, get this, {{firstPerson}}. There was this guy named {{secondPerson}}, right?";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 0);
			Assert.IsTrue(result.StringVariables.Count == 2);
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "firstPerson"));
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "secondPerson"));
		}

		[TestMethod]
		public void StringVariableShouldSupportDescriptions()
		{
			const string input = "I have {{number|Number of pickles}} pickles!";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 0);
			Assert.IsTrue(result.StringVariables.Count == 1);
			Assert.IsTrue(result.StringVariables[0].Name == "number");
			Assert.IsTrue(result.StringVariables[0].Description == "Number of pickles");
		}

		[TestMethod]
		public void ShouldReturnTwoBooleanAndTwoVariables()
		{
			const string input =
				"Back in the 4th of {{month}}, there {{#ifSingular}}was{{/ifSingular}}{{#ifPlural}}were{{/ifPlural}} {{count}} different ways of doing things.";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 2);
			Assert.IsTrue(result.StringVariables.Count == 2);
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "month"));
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "count"));
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifSingular"));
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifPlural"));
		}

		[TestMethod]
		public void ShouldReturnTwoBooleanAndTwoVariablesWithSomeDescriptions()
		{
			const string input =
				"Back in the 4th of {{month|Month in the year}}, there {{#ifSingular}}was{{/ifSingular}}{{#ifPlural|Changes the tense}}were{{/ifPlural}} {{count}} different ways of doing things.";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);

			Assert.IsTrue(result.BooleanLogics.Count == 2);
			Assert.IsTrue(result.StringVariables.Count == 2);
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "month"));
			Assert.IsTrue(result.StringVariables.First(a => a.Name == "month").Description == "Month in the year");
			Assert.IsTrue(result.StringVariables.Any(a => a.Name == "count"));
			Assert.IsTrue(result.StringVariables.First(a => a.Name == "count").Description == "");
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifSingular"));
			Assert.IsTrue(result.BooleanLogics.First(a => a.Name == "ifSingular").Description == "");
			Assert.IsTrue(result.BooleanLogics.Any(a => a.Name == "ifPlural"));
			Assert.IsTrue(result.BooleanLogics.First(a => a.Name == "ifPlural").Description == "Changes the tense");
		}

		[TestMethod]
		public void GetStartIndexShouldYieldNullForNoStartTags()
		{
			const string input = "Four score and seven years ago...";
			var service = new Parser();
			var result = service.GetIndexOfNextOpenTag(input);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetStartIndexShouldYieldNullForNoStartTagsWithStartIndex()
		{
			const string input = "If you can dream it, you can do it.";
			var service = new Parser();
			var result = service.GetIndexOfNextOpenTag(input, 3);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetStartIndexShouldYieldAppropriateIndexValue()
		{
			const string input = "If you can {{verb}} it, you can do it.";
			var service = new Parser();
			var result = service.GetIndexOfNextOpenTag(input);

			Assert.AreEqual(11, result);
		}

		[TestMethod]
		public void GetStartIndexShouldYieldAppropriateIndexValueWithStartIndex()
		{
			const string input = "If you can {{verb}} it, you can do it.";
			var service = new Parser();
			var result = service.GetIndexOfNextOpenTag(input, 5);

			Assert.AreEqual(11, result);
		}

		[TestMethod]
		public void GetEndIndexShouldYieldNullForNoEndTags()
		{
			const string input = "If you can {{verb it, you can do it.";
			var service = new Parser();
			var result = service.GetIndexOfCloseTag(input, 0);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetEndIndexShouldYieldAppropriateIndexValue()
		{
			const string input = "{{verb}} it now!";
			var service = new Parser();
			var result = service.GetIndexOfCloseTag(input, 0);

			Assert.AreEqual(6, result);
		}

		[TestMethod]
		public void ParseWithNoTagsShouldYieldExpectedOutcome()
		{
			const string input = "Four score and seven years ago...";
			var template = new Template
			{
				TextTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);
			var output = service.FinalizeTemplate(result);
			Assert.AreEqual("Four score and seven years ago...", output.PlainEmailBody);
		}

		[TestMethod]
		public void HtmlParseWithNoTagsShouldYieldExpectedOutcome()
		{
			const string input = "<html><h1>Four scope and seven years ago...";
			var template = new Template
			{
				HtmlTemplate = input
			};
			var service = new Parser();
			var result = service.InitializeEmail(template);
			var output = service.FinalizeTemplate(result);
			Assert.AreEqual("<html><h1>Four scope and seven years ago...", output.HtmlEmailBody);
		}

		[TestMethod]
		public void PopulateStringVariablesShouldYieldExpectedOutcome1()
		{
			const string input = "I {{verb}} to the store.";
			var variables = new List<StringVariable>
			{
				new StringVariable
				{
					Name = "verb",
					Value = "ran"
				}
			};

			var service = new Parser();
			var result = service.PopulateStringVariables(variables, input);

			Assert.AreEqual("I ran to the store.", result);
		}

		[TestMethod]
		public void PopulateHtmlVariablesShouldYieldExpectedOutcome1()
		{
			const string input = "<html>I {{verb}} to the <b>store</b>.</html>";
			var variables = new List<StringVariable>
			{
				new StringVariable
				{
					Name = "verb",
					Value = "walked"
				}
			};

			var service = new Parser();
			var result = service.PopulateStringVariables(variables, input);

			Assert.AreEqual("<html>I walked to the <b>store</b>.</html>", result);
		}

		[TestMethod]
		public void PopulateStringVariablesShouldYieldExpectedOutcome2()
		{
			const string input = "I {{modifier}} {{verb}} to the store.";
			var variables = new List<StringVariable>
			{
				new StringVariable
				{
					Name = "modifier",
					Value = "often"
				},
				new StringVariable
				{
					Name = "verb",
					Value = "jog"
				}
			};

			var service = new Parser();
			var result = service.PopulateStringVariables(variables, input);

			Assert.AreEqual("I often jog to the store.", result);
		}

		[TestMethod]
		public void PopulateStringVariablesShouldYieldExpectedOutcome3()
		{
			const string input = "I {{modifier}} {{verb}} to the {{place}}.";
			var variables = new List<StringVariable>
			{
				new StringVariable
				{
					Name = "modifier",
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
			};

			var service = new Parser();
			var result = service.PopulateStringVariables(variables, input);

			Assert.AreEqual("I usually hike to the park.", result);
		}

		[TestMethod]
		public void PopulateDuplicateStringVariablesShouldYieldExpectedOutcome4()
		{
			const string input = "I {{modifier}} {{verb}} to the {{place}}. Yes, I {{verb}}.";
			var variables = new List<StringVariable>
			{
				new StringVariable
				{
					Name = "modifier",
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
			};

			var service = new Parser();
			var result = service.PopulateStringVariables(variables, input);

			Assert.AreEqual("I usually hike to the park. Yes, I hike.", result);
		}

		[TestMethod]
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

		[TestMethod]
		public void PopulateDuplicateBooleanVariablesShouldYieldExpectedOutcome1()
		{
			const string input = "I {{#have}}was awesome{{/have}}{{#dontHave}}got tired{{/dontHave}}{{#have}} over there{{/have}}!";
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

			Assert.AreEqual("I was awesome over there!", result);
		}

		[TestMethod]
		public void PopulateBooleanVariablesShouldYieldExpectedOutcome2()
		{
			const string input = "I {{#have}}was awesome{{/have}}{{#dontHave}}got tired{{/dontHave}}!";
			var variables = new List<BooleanLogic>
			{
				new BooleanLogic
				{
					Name = "have",
					Value = false
				},
				new BooleanLogic
				{
					Name = "dontHave",
					Value = true
				}
			};

			var service = new Parser();
			var result = service.PopulateBooleanVariables(variables, input);

			Assert.AreEqual("I got tired!", result);
		}

		[TestMethod]
		public void PopulateBooleanVariablesShouldYieldExpectedOutcome3()
		{
			const string input = "I {{#have}}was {{#extra}}really {{/extra}}awesome{{/have}}{{#dontHave}}got tired{{/dontHave}}!";
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
				},
				new BooleanLogic
				{
					Name = "extra",
					Value = true
				}
			};

			var service = new Parser();
			var result = service.PopulateBooleanVariables(variables, input);

			Assert.AreEqual("I was really awesome!", result);
		}

		[TestMethod]
		public void CombineVariablesTest()
		{
			const string plainInput =
				"Don't {{verb}} each {{#repeater}}and every {{/repeater}}{{time}} by the {{noun}} you reap{{#includeRest}}, but by the {{otherNoun}}s that you plant{{/includeRest}}.";
			const string htmlInput =
				"<html><head></head><body><h1>Don't</h1> {{verb}} each {{#repeater}}and every {{/repeater}}{{time}}.</body></html>";
			var template = new Template
			{
				Subject = "This is my {{noun}}",
				HtmlTemplate = htmlInput,
				TextTemplate = plainInput
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
			Assert.AreEqual("<html><head></head><body><h1>Don't</h1> judge each day.</body></html>", output.HtmlEmailBody);
			Assert.AreEqual("This is my harvest", output.Subject);
		}

		[TestMethod]
		public void CombineDuplicateVariablesTest()
		{
			const string input =
				"Don't {{verb}} each {{#repeater}}and every {{/repeater}}{{time}} by the {{noun}} you reap{{#includeRest}}, but by the {{otherNoun}}s that you plant{{/includeRest}}. Yes, {{otherNoun}}s.";
			var template = new Template
			{
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
			Assert.AreEqual("Don't judge each day by the harvest you reap, but by the seeds that you plant. Yes, seeds.", output.PlainEmailBody);
		}

		[TestMethod]
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
	}
}
