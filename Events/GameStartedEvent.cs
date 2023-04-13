using System;
using System.Collections.Generic;
using Events;

namespace Events;

public class GameStartedEvent : TracingEventBase
{
    public Guid GameId { get; set; }
}