﻿using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace Xml2CSharp.Clent
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			if (!Parser.Default.ParseArguments(args, options))
				return;

			var xml = File.ReadAllText(options.XmlFile);

			var classInfo = new Xml2CSharpConverer().Convert(xml);

			var classInfoWriter = new ClassInfoWriter(classInfo, options.CustomNameSpace);

			if (string.IsNullOrEmpty(options.CSharpFileName))
			{
				classInfoWriter.Write(Console.Out);
			}

			using (var sw = new StreamWriter(File.OpenWrite(options.CSharpFileName)))
			{
				classInfoWriter.Write(sw);
			}
		}
	}

	class Options
	{
		[Option('x', "xmlFile", Required = true,
		  HelpText = "Xml file used to create c# files.")]
		public string XmlFile { get; set; }

		[Option('c', "cSharpFileName", DefaultValue = "classes.cs",
		  HelpText = "Name of C# file to be created")]
		public string CSharpFileName { get; set; }

		[Option('n', "namespace", DefaultValue = null,
			HelpText = "Custom Namespace to use in output class files")]
		public string CustomNameSpace { get; set; }

		//public bool OutputToConsole { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
			  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
