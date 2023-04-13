﻿using System;
using System.Collections.Generic;

namespace Events;

public class GameStartedEvent
{
    public Dictionary<string, object> Header { get; set; } = new();
    public Guid GameId { get; set; }
}