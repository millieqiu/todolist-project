﻿using System;
using System.Collections.Generic;

namespace todoAPP.Models
{
    public partial class Todo
    {
        public int Idx { get; set; }
        public Guid Uid { get; set; }
        public byte Status { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Weather { get; set; } = null!;
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
