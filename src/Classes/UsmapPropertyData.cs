using UsmapNET.Enums;

namespace UsmapNET.Classes
{
	public class UsmapPropertyData
	{
		public EUsmapPropertyType Type { get; }
		public string EnumName { get; set; }
		public string StructType { get; set; }
		public UsmapPropertyData InnerType { get; set; }
		public UsmapPropertyData ValueType { get; set; }

		internal UsmapPropertyData(EUsmapPropertyType type)
		{
			Type = type;
		}
	}
}