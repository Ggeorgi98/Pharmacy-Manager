﻿namespace PharmManager.Orders.Domain
{
    public class Paginator
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public Paginator()
        {
            CurrentPage = 1;
            PageSize = 25;
        }
    }
}
