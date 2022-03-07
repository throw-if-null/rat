namespace Rat.Data
{
    public enum ProcessingStatus
    {
        None = 0,
        Ok = 1,
        GoodRequest = 2,
        BadRequest = 4,
        NotFound = 8,
		Error = 32
	}
}
