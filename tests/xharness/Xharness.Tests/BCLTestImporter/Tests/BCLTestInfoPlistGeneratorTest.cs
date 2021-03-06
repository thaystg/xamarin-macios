using System;
using System.IO;

using NUnit.Framework;

using BCLTestImporter;
using System.Threading.Tasks;

namespace Xharness.Tests.BCLTestImporter.Tests {
	public class BCLTestInfoPlistGeneratorTest {

		[Test]
		public void GenerateCodeNullTemplateFile ()
		{
			Assert.ThrowsAsync<ArgumentNullException> (() => 
				BCLTestInfoPlistGenerator.GenerateCodeAsync (null, "Project Name"));
		}

		[Test]
		public void GenerateCodeNullProjectName ()
		{
			Assert.ThrowsAsync <ArgumentNullException> (() =>
				BCLTestInfoPlistGenerator.GenerateCodeAsync ("A/path", null));
		}

		[Test]
		public async Task GenerateCode ()
		{
			const string projectName = "MyTest";
			var fakeTemplate = $"{BCLTestInfoPlistGenerator.ApplicationNameReplacement}-{BCLTestInfoPlistGenerator.IndentifierReplacement}";
			var tmpPath = Path.GetTempPath ();
			var templatePath = Path.Combine (tmpPath, Path.GetRandomFileName());
			using (var file = new StreamWriter (templatePath, false)) { 
				await file.WriteAsync (fakeTemplate);
			}

			var result = await BCLTestInfoPlistGenerator.GenerateCodeAsync (templatePath, projectName);
			try {
				StringAssert.DoesNotContain (BCLTestInfoPlistGenerator.ApplicationNameReplacement, result);
				StringAssert.DoesNotContain (BCLTestInfoPlistGenerator.IndentifierReplacement, result);
				StringAssert.Contains (projectName, result);
			}
			finally {
				File.Delete (templatePath);
			}
		}
	}
}
