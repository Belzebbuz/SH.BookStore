﻿namespace SH.Bookstore.Books.Infrastructure.Auditing;
internal enum TrailType : byte
{
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3
}