using EasyNetQ;

namespace Helpers;

public static class ConnectionHelper
{
    public static IBus GetRMQConnection()
    {
        return RabbitHutch.CreateBus("host=localhost;username=mina0825@easv365.dk;password=CIRkeline2");
    }
}