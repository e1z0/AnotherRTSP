using System;
namespace Tiny {

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class JsonPropertyAttribute : Attribute {
		public string Name { get; private set; }

		public JsonPropertyAttribute(string name) {
			Name = name;
		}
	}
}
