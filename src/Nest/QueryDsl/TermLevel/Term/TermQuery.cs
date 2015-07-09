﻿using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof (FieldNameQueryConverter<TermQuery>))]
	public interface ITermQuery : IFieldNameQuery
	{
		[JsonProperty("value")]
		object Value { get; set; }
	}

	public class TermQuery : FieldNameQuery, ITermQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public object Value { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.Term = this;
		internal static bool IsConditionless(ITermQuery q) => q.Value == null || q.Value.ToString().IsNullOrEmpty() || q.Field.IsConditionless();
	}

	public class TermQueryDescriptorBase<TDescriptor, T> 
		: FieldNameQueryDescriptor<TermQueryDescriptorBase<TDescriptor, T>, ITermQuery, T>
		, ITermQuery
		where TDescriptor : TermQueryDescriptorBase<TDescriptor, T>
		where T : class
	{
		private ITermQuery Self => this;
		bool IQuery.Conditionless => TermQuery.IsConditionless(this);
		object ITermQuery.Value { get; set; }

		public TDescriptor Value(object value)
		{
			Self.Value = value;
			return (TDescriptor)this;
		}
	}

	public class TermQueryDescriptor<T> : TermQueryDescriptorBase<TermQueryDescriptor<T>, T>
		where T : class
	{	
	}
}
