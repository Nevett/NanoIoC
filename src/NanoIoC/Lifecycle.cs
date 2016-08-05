namespace NanoIoC
{
    public enum Lifecycle
    {
		Transient = 1,
		HttpContextOrThreadLocal = 2,
        Singleton = 3,
		ExecutionContextLocal = 4,
    }
}