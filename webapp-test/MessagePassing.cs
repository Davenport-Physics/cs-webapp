
using System.Threading.Channels;

static class MessagePassing {

    private static readonly object lock_obj = new();
    private static int unique_id = 0;

    private static List<UniqueChannel> channels = [];

    public static Subscription Subscribe() {

        lock(lock_obj) {
            
            var channel = Channel.CreateUnbounded<UserMessage>();
            var id      = unique_id++;

            channels.Add(new UniqueChannel(id, channel));
            return new Subscription(id, channel);

        }

    }

    public static void Unsubscribe(int id) {

        lock(lock_obj) {

            var idx = channels.FindIndex((channel) => {
                return id == channel.Id;
            });

            if (idx != -1) {
                channels.RemoveAt(idx);
            }

        }

    }

    public static void Publish(UserMessage message) {

        lock(lock_obj) {

            foreach (var channel in channels) {

                channel.Channel.Writer.TryWrite(message);

            }

        }

    }

}

class Subscription(int id, ChannelReader<UserMessage> reader)
{
    private readonly int id = id;
    private readonly ChannelReader<UserMessage> reader = reader;

    public void Unsubscribe() {

        MessagePassing.Unsubscribe(id);

    }

    public async Task<UserMessage?> Read() {

        return await reader.ReadAsync();

    }

}

record UniqueChannel(int Id, Channel<UserMessage> Channel);