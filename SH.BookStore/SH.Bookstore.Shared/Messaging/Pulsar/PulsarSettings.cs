namespace SH.Bookstore.Shared.Messaging.Pulsar;
public class PulsarSettings
{
    public string SeriviceUrl { get; set; } = "pulsar://127.0.0.1:6650";
    public string ShiftOrderCreateTopic { get; set; } = nameof(ShiftOrderCreateTopic);
    public string ShiftOrderUpdatedTopic { get; set; } = nameof(ShiftOrderUpdatedTopic);
    public string BookCreatedTopic { get; set; } = nameof(BookCreatedTopic);
}
