﻿using System;

namespace Models.Order
{
    public class OrderRequestDto : BaseModel
    {
        public DateTime Date = DateTime.Now;
        public int IdStatus { get; set; }
        public int IdUser { get; set; }
        public decimal Cost { get; set; }
    }
}