﻿using System;
using Elastic.Xunit.XunitPlumbing;
using FluentAssertions;
using Nest;
using Tests.Core.Extensions;
using Tests.Core.Serialization;

namespace Tests.Reproduce
{
	public class GithubIssue4057
	{
		[U]
		[UseCulture("sv-SE")]
		public void DoubleAffectedByPrecisionProblemDeserializesCorrectlyIndependentOfCurrentCulture()
		{
			var expected = 16.27749494276941D;

			var tester = SerializationTester.Default;
			var actual = tester.Deserializes<double>("16.27749494276941");

			actual.Result.Should().NotBe(expected); // we are expecting precision problem;
			Math.Round(actual.Result, 13).Should().Be(Math.Round(expected, 13));
		}
	}
}
