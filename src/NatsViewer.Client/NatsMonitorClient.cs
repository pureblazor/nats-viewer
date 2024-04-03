using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace NatsViewer.Client;

public class NatsMonitorClient(ILogger<NatsMonitorClient> log, HttpClient client)
{
    public async Task<HealthCheckResult> HealthCheck()
    {
        try
        {
            var response = await client.GetAsync("/healthz");
            return new HealthCheckResult
            {
                Healthy = response.IsSuccessStatusCode
            };
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to health check");
            return new HealthCheckResult
            {
                Healthy = false
            };
        }
    }
    
    public async Task<ServerInfo?> GetServerInfo()
    {
        try
        {
            return await client.GetFromJsonAsync<ServerInfo>("/varz");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to get server info");
            return null;
        }
    }
    
    public async Task<ExpandedJetStreamInfo?> GetExpandedJetStreamInfo()
    {
        try
        {
            return await client.GetFromJsonAsync<ExpandedJetStreamInfo>("/jsz?consumers=true");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to get expanded jetstream info");
            return null;
        }
    }
}

public record HealthCheckResult
{
    public bool Healthy { get; set; }
}
public record ServerInfo
{
    [JsonPropertyName("server_id")]
    public string? ServerId { get; init; }

    [JsonPropertyName("server_name")]
    public string? ServerName { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }

    [JsonPropertyName("proto")]
    public int Proto { get; init; }

    [JsonPropertyName("git_commit")]
    public string? GitCommit { get; init; }

    [JsonPropertyName("go")]
    public string? Go { get; init; }

    [JsonPropertyName("host")]
    public string? Host { get; init; }

    [JsonPropertyName("port")]
    public int Port { get; init; }

    [JsonPropertyName("max_connections")]
    public int MaxConnections { get; init; }

    [JsonPropertyName("ping_interval")]
    public long PingInterval { get; init; }

    [JsonPropertyName("ping_max")]
    public int PingMax { get; init; }

    [JsonPropertyName("http_host")]
    public string? HttpHost { get; init; }

    [JsonPropertyName("http_port")]
    public int HttpPort { get; init; }

    [JsonPropertyName("http_base_path")]
    public string? HttpBasePath { get; init; }

    [JsonPropertyName("https_port")]
    public int HttpsPort { get; init; }

    [JsonPropertyName("auth_timeout")]
    public int AuthTimeout { get; init; }

    [JsonPropertyName("max_control_line")]
    public int MaxControlLine { get; init; }

    [JsonPropertyName("max_payload")]
    public int MaxPayload { get; init; }

    [JsonPropertyName("max_pending")]
    public int MaxPending { get; init; }

    [JsonPropertyName("jetstream")]
    public JetStreamInfo? JetStream { get; init; }

    [JsonPropertyName("tls_timeout")]
    public int TlsTimeout { get; init; }

    [JsonPropertyName("write_deadline")]
    public long WriteDeadline { get; init; }

    [JsonPropertyName("start")]
    public string? Start { get; init; }

    [JsonPropertyName("now")]
    public string? Now { get; init; }

    [JsonPropertyName("uptime")]
    public string? Uptime { get; init; }

    [JsonPropertyName("mem")]
    public int Mem { get; init; }

    [JsonPropertyName("cores")]
    public int Cores { get; init; }

    [JsonPropertyName("gomaxprocs")]
    public int Gomaxprocs { get; init; }

    [JsonPropertyName("cpu")]
    public int Cpu { get; init; }

    [JsonPropertyName("connections")]
    public int Connections { get; init; }

    [JsonPropertyName("total_connections")]
    public int TotalConnections { get; init; }

    [JsonPropertyName("routes")]
    public int Routes { get; init; }

    [JsonPropertyName("remotes")]
    public int Remotes { get; init; }

    [JsonPropertyName("leafnodes")]
    public int Leafnodes { get; init; }

    [JsonPropertyName("in_msgs")]
    public int InMsgs { get; init; }

    [JsonPropertyName("out_msgs")]
    public int OutMsgs { get; init; }

    [JsonPropertyName("in_bytes")]
    public int InBytes { get; init; }

    [JsonPropertyName("out_bytes")]
    public int OutBytes { get; init; }

    [JsonPropertyName("slow_consumers")]
    public int SlowConsumers { get; init; }

    [JsonPropertyName("subscriptions")]
    public int Subscriptions { get; init; }

    [JsonPropertyName("http_req_stats")]
    public Dictionary<string, int>? HttpReqStats { get; init; }

    [JsonPropertyName("config_load_time")]
    public string? ConfigLoadTime { get; init; }

    [JsonPropertyName("system_account")]
    public string? SystemAccount { get; init; }

    [JsonPropertyName("slow_consumer_stats")]
    public SlowConsumerStats? SlowConsumerStats { get; init; }
}

public record JetStreamInfo
{
    [JsonPropertyName("config")]
    public JetStreamConfig? Config { get; init; }

    [JsonPropertyName("stats")]
    public JetStreamStats? Stats { get; init; }
}
public record JetStreamConfig
{
    [JsonPropertyName("max_memory")]
    public long MaxMemory { get; init; }

    [JsonPropertyName("max_storage")]
    public long MaxStorage { get; init; }

    [JsonPropertyName("store_dir")]
    public string? StoreDir { get; init; }

    [JsonPropertyName("sync_interval")]
    public long SyncInterval { get; init; }
}

public record JetStreamStats
{
    [JsonPropertyName("memory")]
    public int Memory { get; init; }

    [JsonPropertyName("storage")]
    public int Storage { get; init; }

    [JsonPropertyName("reserved_memory")]
    public int ReservedMemory { get; init; }

    [JsonPropertyName("reserved_storage")]
    public int ReservedStorage { get; init; }

    [JsonPropertyName("accounts")]
    public int Accounts { get; init; }

    [JsonPropertyName("ha_assets")]
    public int HaAssets { get; init; }

    [JsonPropertyName("api")]
    public ApiStats? Api { get; init; }
}

public record ApiStats
{
    [JsonPropertyName("total")]
    public int Total { get; init; }

    [JsonPropertyName("errors")]
    public int Errors { get; init; }
}

public record SlowConsumerStats
{
    [JsonPropertyName("clients")]
    public int Clients { get; init; }

    [JsonPropertyName("routes")]
    public int Routes { get; init; }

    [JsonPropertyName("gateways")]
    public int Gateways { get; init; }

    [JsonPropertyName("leafs")]
    public int Leafs { get; init; }
}

public record ExpandedJetStreamInfo(
    [property: JsonPropertyName("server_id")] string ServerId,
    [property: JsonPropertyName("now")] DateTime Now,
    [property: JsonPropertyName("config")] JetStreamConfig Config,
    [property: JsonPropertyName("memory")] long Memory,
    [property: JsonPropertyName("storage")] long Storage,
    [property: JsonPropertyName("reserved_memory")] long ReservedMemory,
    [property: JsonPropertyName("reserved_storage")] long ReservedStorage,
    [property: JsonPropertyName("accounts")] long Accounts,
    [property: JsonPropertyName("ha_assets")] long HaAssets,
    [property: JsonPropertyName("api")] Api Api,
    [property: JsonPropertyName("streams")] long Streams,
    [property: JsonPropertyName("consumers")] long Consumers,
    [property: JsonPropertyName("messages")] long Messages,
    [property: JsonPropertyName("bytes")] long Bytes,
    [property: JsonPropertyName("account_details")] List<AccountDetail> AccountDetails
);

public record Api(
    [property: JsonPropertyName("total")] long Total,
    [property: JsonPropertyName("errors")] long Errors
);

public record AccountDetail(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("memory")] long Memory,
    [property: JsonPropertyName("storage")] long Storage,
    [property: JsonPropertyName("reserved_memory")] Int128 ReservedMemory,
    [property: JsonPropertyName("reserved_storage")] Int128 ReservedStorage,
    [property: JsonPropertyName("accounts")] long Accounts,
    [property: JsonPropertyName("ha_assets")] long HaAssets,
    [property: JsonPropertyName("api")] Api Api,
    [property: JsonPropertyName("stream_detail")] List<StreamDetail> StreamDetail
);

public record StreamDetail(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("cluster")] Cluster Cluster,
    [property: JsonPropertyName("state")] State State,
    [property: JsonPropertyName("consumer_detail")] List<ConsumerDetail>? ConsumerDetail
);

public record Cluster(
    [property: JsonPropertyName("leader")] string Leader
);

public record State(
    [property: JsonPropertyName("messages")] long Messages,
    [property: JsonPropertyName("bytes")] long Bytes,
    [property: JsonPropertyName("first_seq")] long FirstSeq,
    [property: JsonPropertyName("first_ts")] DateTime FirstTs,
    [property: JsonPropertyName("last_seq")] long LastSeq,
    [property: JsonPropertyName("last_ts")] DateTime? LastTs,
    [property: JsonPropertyName("consumer_count")] long ConsumerCount,
[property: JsonPropertyName("num_subjects")] long NumSubjects
);

public record ConsumerDetail(
    [property: JsonPropertyName("stream_name")] string StreamName,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("created")] DateTime Created,
    [property: JsonPropertyName("delivered")] Delivered Delivered,
    [property: JsonPropertyName("ack_floor")] AckFloor AckFloor,
    [property: JsonPropertyName("num_ack_pending")] long NumAckPending,
    [property: JsonPropertyName("num_redelivered")] long NumRedelivered,
    [property: JsonPropertyName("num_waiting")] long NumWaiting,
    [property: JsonPropertyName("num_pending")] long NumPending,
    [property: JsonPropertyName("ts")] DateTime? Ts
);

public record Delivered(
    [property: JsonPropertyName("consumer_seq")] long ConsumerSeq,
    [property: JsonPropertyName("stream_seq")] long StreamSeq,
    [property: JsonPropertyName("last_active")] DateTime? LastActive
);

public record AckFloor(
    [property: JsonPropertyName("consumer_seq")] long ConsumerSeq,
    [property: JsonPropertyName("stream_seq")] long StreamSeq,
    [property: JsonPropertyName("last_active")] DateTime? LastActive
);