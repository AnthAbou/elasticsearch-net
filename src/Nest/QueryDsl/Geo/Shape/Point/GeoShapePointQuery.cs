﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nest.Resolvers;
using Newtonsoft.Json;
using Elasticsearch.Net;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IGeoShapePointQuery : IGeoShapeQuery
	{
		[JsonProperty("shape")]
		IPointGeoShape Shape { get; set; }
	}

	public class GeoShapePointQuery : FieldNameQuery, IGeoShapePointQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public IPointGeoShape Shape { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.GeoShape = this;
		internal static bool IsConditionless(IGeoShapePointQuery q) => q.Field.IsConditionless() || q.Shape == null || !q.Shape.Coordinates.HasAny();
	}

	public class GeoShapePointQueryDescriptor<T> 
		: FieldNameQueryDescriptor<GeoShapePointQueryDescriptor<T>, IGeoShapePointQuery, T>
		, IGeoShapePointQuery where T : class
	{
		private IGeoShapePointQuery Self => this;
		bool IQuery.Conditionless => GeoShapePointQuery.IsConditionless(this);
		IPointGeoShape IGeoShapePointQuery.Shape { get; set; }
		
		public GeoShapePointQueryDescriptor<T> Coordinates(IEnumerable<double> coordinates)
		{
			if (Self.Shape == null)
				Self.Shape = new PointGeoShape();
			Self.Shape.Coordinates = coordinates;
			return this;
		}
	}
}
