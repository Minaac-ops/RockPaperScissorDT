using System;
using System.Collections.Generic;
using Events;

namespace Events;

public class GameFinishedEvent : TracingEventBase
{
    public Guid GameId { get; set; }
    public string? WinnerId { get; set; }
    public Dictionary<string, Move> Moves { get; set; } = new Dictionary<string, Move>();
}