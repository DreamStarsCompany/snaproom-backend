﻿namespace SnapRoom.Common.Utils
{
	[AttributeUsage(AttributeTargets.All)]
	public class CustomName : Attribute
	{
		public string Name { get; set; }
		public CustomName(string name)
		{
			Name = name;
		}
	}
}
