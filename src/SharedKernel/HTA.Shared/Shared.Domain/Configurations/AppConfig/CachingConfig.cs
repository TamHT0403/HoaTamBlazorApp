namespace Shared.Domain
{
    public class CachingConfig
    {
        public RedisConfig RedisConfig { get; set; } = null!;
    }

    public class RedisConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
    }
}