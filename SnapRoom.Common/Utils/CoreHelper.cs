namespace SnapRoom.Common.Utils
{
    public class CoreHelper
	{
		public static DateTimeOffset SystemTimeNow => TimeHelper.ConvertToUtcPlus7(DateTimeOffset.Now);

	}
}
